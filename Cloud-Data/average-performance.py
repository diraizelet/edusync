# Import Spark SQL functions
from pyspark.sql import SparkSession
from pyspark.sql.functions import avg, count, col, to_date, sum as sum_

# Create Spark session
spark = SparkSession.builder.appName("EduSyncQuizAnalytics").getOrCreate()

# Read Results data from Azure Data Lake or Synapse table
results_df = spark.read \
    .format("com.databricks.spark.sqldw") \
    .option("url", "jdbc:sqlserver://<server>.database.windows.net:1433;database=<db>;user=<user>;password=<password>") \
    .option("forwardSparkAzureStorageCredentials", "true") \
    .option("tempDir", "wasbs://<container>@<storage_account>.blob.core.windows.net/temp") \
    .option("dbTable", "Results") \
    .load()

# Example pass threshold
PASS_THRESHOLD = 50

# 1. Average score per quiz
avg_score_df = results_df.groupBy("AssessmentId").agg(avg("Score").alias("AverageScore"))

# 2. Pass rate per quiz
pass_rate_df = results_df.withColumn("Passed", col("Score") >= PASS_THRESHOLD) \
    .groupBy("AssessmentId") \
    .agg(
        count("*").alias("TotalAttempts"),
        sum_(col("Passed").cast("int")).alias("PassedCount")
    ) \
    .withColumn("PassRate", col("PassedCount") / col("TotalAttempts") * 100)

# 3. Daily performance trend (all quizzes)
trend_df = results_df.withColumn("Date", to_date("AttemptDate")) \
    .groupBy("Date") \
    .agg(avg("Score").alias("AvgScorePerDay"))

# Show results
avg_score_df.show()
pass_rate_df.show()
trend_df.show()

# Optionally: Save to Synapse or Azure SQL for Power BI
avg_score_df.write.mode("overwrite").saveAsTable("QuizAverageScores")
pass_rate_df.write.mode("overwrite").saveAsTable("QuizPassRates")
trend_df.write.mode("overwrite").saveAsTable("DailyScoreTrends")
