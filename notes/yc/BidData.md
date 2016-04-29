[TOC]

## BigData

### Solution
+ ELK (ElasticSearch, Logstash, Kipana)
	> monitoring/diagnostics against log file sources etc.
##### Log
+ graylog
+ flume

##### ETL
+ Talend Open Studio
+ Pentaho Kettle

##### Monitor
+ open-falcon from Xiaomi
+ Grafana+influxDB+collectd
	>  purely time series metrics (specifically, monitoring applications & servers)
+ Grafana+Zabbix
	> Grafana + Zabbix datasource
##### One-stop

+ databricks https://community.cloud.databricks.com/?o=1816859815744643
+ oneops oneops.com

### Glossary
  + HDFS - Hadoop Distributed File System
  > Storage Layer, distributed, scalable, java-based, large volumes of unstructured data
  + MapReduce
  > Compute Layer, software framework. Jobs, Map function, Reduce function
  + Hive
  > a framework, Hadoop-based warehousing like. HiveSQL: SQL like language,
  > convert to MapReduce to query Hadoop
  + Pig
  > Hadoop-based language, for data pipelines
  + HBase
  > non-relational database, open source implementation of Googl BigTable. Column DB
  lookups in Hadoop, add transaction capability on Hadoop
  + Flume
  > framework, populate data Hadoop with data.
  > could be used to collect logs, agent(file,syslog), collector, storage(file, HDFS)
  + Oozie
  > workflow processing system, support multiple language. similar to Aether
  + Ambari
  > web-based tool, to depoy/manage/monitor Hadoop cluster
  + Avro
  > RPC and data serialization framework
  > no need run code-gen when schema Change
  > similar to Thrift/ProtocolBuffer
  + Mahout
  > data mining lib, implement modelling using Map Reduce model
  + Sqoop
  > connective tool, move data from non-Hadoop data store to Hadoop
  + HCatalog
  + BigTop
  + Zookeeper
  > Provide distributed configuration service, synchronization service and
  > naming registry
  + Storm
  + Kafka
  + Spark
  + Mesos
  > abstract compute resoure (CPU, memory, storage) from machines (physical or virtual)
  >
  + Docker
  + Kubernetes
  + ElasticSearch
  + Jenkins
	+ Zabbix
	> enterprise-level software designed for real-time monitoring of millions of metrics collected from tens of thousands of servers, virtual machines and network devices

### Hadoop Ecosystem
  ![Hadoop Ecocsystem](http://www.searchtb.com/wp-content/uploads/2011/01/image0010.jpg)


```flow
st=>start: Start:>http://www.google.com[blank]
e=>end:>http://www.google.com
op1=>operation: My Operation
sub1=>subroutine: My Subroutine
cond=>condition: Yes
or No?:>http://www.google.com
io=>inputoutput: catch something...

st->op1->cond
cond(yes)->io->e
cond(no)->sub1(right)->op1
```
