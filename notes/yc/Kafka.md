## Kafka

### Problem Description
```
The data is all going to one kafka partition.  
Reading the below passage I realize we are doing it wrong.  
We need to provide a key that will give random distribution of bids (usec timestamp maybe).  
We also need to set the partition key correctly for impressions, clicks and beacons.  
For impressions and clicks the key should be tactic id.  For beacons random key is also ok.

This is a description of the problem from the kafka faq:

Why is data not evenly distributed among partitions when a partitioning key is not specified?
In Kafka producer, a partition key can be specified to indicate the destination partition of the message. 
By default, a hashing-based partitioner is used to determine the partition id given the key, 
and people can use customized partitioners also.

To reduce # of open sockets, in 0.8.0 (https://issues.apache.org/jira/browse/KAFKA-1017), 
when the partitioning key is not specified or null, a producer will pick a random partition 
and stick to it for some time (default is 10 mins) before switching to another one. 
So, if there are fewer producers than partitions, at a given point of time, some partitions may not receive any data. 
To alleviate this problem, one can either reduce the metadata refresh interval or specify a 
message key and a customized random partitioner. 

For more detail see this thread http://mail-archives.apache.org/mod_mbox/kafka-dev/201310.mbox/%3CCAFbh0Q0aVh%2Bvqxfy7H-%2BMnRFBt6BnyoZk1LWBoMspwSmTqUKMg%40mail.gmail.com%3E
```

### Todo
#### For bids
+ Get key to give random distribution of bids
+ Think of how to get the key, maybe by timestamp

#### For adImpression, adClick and beacon
+ Get key to give random distribution of 
+ The key should related to tactic, b/c we want the impression and click of one tactic in one partition

### Related Files
+ ./src/main/java/com/yosemitecloud/rts/main/server/eventrecord/EventRecorder.java

