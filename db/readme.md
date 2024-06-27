clickhouse-client --query "CREATE TABLE driver_created_queue (Id UUID, Name String, CarType String, HorsePower Int, Timestamp UInt64) ENGINE = Kafka('broker:19092', 'DriverCreated', 'consumer-group-1', 'JSONEachRow');"

clickhouse-client --query "CREATE TABLE daily ( day Date, total UInt64 ) ENGINE = SummingMergeTree() ORDER BY (day);"

clickhouse-client --query "CREATE MATERIALIZED VIEW consumer TO daily AS SELECT toDate(toDateTime(Timestamp)) AS day, count() as total FROM default.driver_created_queue GROUP BY day;"