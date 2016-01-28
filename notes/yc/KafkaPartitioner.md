## Kafka Partition

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
To alleviate this problem, one can either reduce the metadata refresh interval or specify a message key and a customized random partitioner. 

For more detail see this thread http://mail-archives.apache.org/mod_mbox/kafka-dev/201310.mbox/%3CCAFbh0Q0aVh%2Bvqxfy7H-%2BMnRFBt6BnyoZk1LWBoMspwSmTqUKMg%40mail.gmail.com%3E
```

### Todo
+ Implement a custom partitioner for BidRequest, random each time

    see [Partitioner Sample](https://cwiki.apache.org/confluence/display/KAFKA/0.8.0+Producer+Example)
```JAVA
import kafka.producer.Partitioner;
import kafka.utils.VerifiableProperties;
 
public class SimplePartitioner implements Partitioner {
    public SimplePartitioner (VerifiableProperties props) {
 
    }
 
    public int partition(Object key, int a_numPartitions) {
        int partition = 0;
        String stringKey = (String) key;
        int offset = stringKey.lastIndexOf('.');
        if (offset > 0) {
           partition = Integer.parseInt( stringKey.substring(offset+1)) % a_numPartitions;
        }
       return partition;
  }
 
}
```

+ ConfigWrapper.getKafkaProperties
+ BidRequestHandler.doPost [Done]
    `invoke addByKey() instead of add()`
+ EventRecord.addByKey()   [Done]
```JAVA
    KeyedMessage<String, String> data = new KeyedMessage<String, String>("page_visits", ip, msg);
    producer.send(data);
```

### Question
+ Q: Does the parition number (for impression, click) is fixed? Where can get it?
    A: the partitioner interface will havae a parameter
    ```JAVA
    ```

#### For bids
+ Get key to give random distribution of bids
+ Think of how to get the key, maybe by timestamp

#### For adImpression, adClick and beacon
+ Get key to give random distribution of 
+ The key should related to tactic, b/c we want the impression and click of one tactic in one partition

### Related Files
+ ./src/main/java/com/yosemitecloud/rts/main/server/eventrecord/EventRecorder.java

### BidRequest Flow
```JAVA
// track the eventRecorder
JettyServerWrapper()
    eventRecorder = new EventRecorder(configWrapper, KafkaTopic.RTBLOG);

JettyServerWrapper.createAndRegisterBidRequestHandlers()
    final BidRequestHandler servlet = new BidRequestHandler(serverId, contextSupplier, exchange, adapter, eventRecorder, false);

BidRequestHandler.doPost
    eventRecorder.add(new BidRequestEvent(bidSlot));

EventRecorder()
    ProducerConfig config = new ProducerConfig(configWrapper.getKafkaProperties());

ConfigWrapper.getKafkaProperties()
        metadata.broker.list
        producer.type
        batch.num.messages

EventRecorder.add(event)
    KeyedMessage<String, byte[]> message = new KeyedMessage<>(topic, msg);
    producer.send(message)
```
    
### Kafka 
+ Producer
    * post message to topic, and can choose which parition to send.
    * can choose parition algorithm, like "round-robin"
    * Producer workflow
        1. how to find the cluster, use Properties
            - **metadata.broker.list**, where to find brokers, at least two
            - **serializer.class**, what Serializer to use when preparing the message for broker
            - **key.serializer.class**, set the Key of the messge, by default it is same to serializer.class
            - **partitioner.class**, determine which partition in the topic the message is to be sent to
                + if include a value for the key, but not define the paritioner.class, Kafka will use default partioner
                + if key is null, Producer will assign the message to a random Parition 
                
                `A producer will pick a random partition and stick to it for some time (default is 10 mins) before switching to another one). If there are fewer producers than partitions, at a given point of time, some partitions may not receive any data. To alleviate this problem, one can either reduce the metadata refresh interval or specify a message key and a customized random partitioner.`

            - **equest.required.acks**, whether want the producer to require an acknowledgement from broker when message received
```JAVA
Properties props = new Properties();
 
props.put("metadata.broker.list", "broker1:9092,broker2:9092");
props.put("serializer.class", "kafka.serializer.StringEncoder");
props.put("partitioner.class", "example.producer.SimplePartitioner");
props.put("request.required.acks", "1");
 
ProducerConfig config = new ProducerConfig(props);
```

+ Consumer
    + need to store the message offset, control how to store and use message
+ kafka cluster
    + one kafka instance means broker
    + zookeeper to ensure cluster/producer/consumer system availability and meta info
+ Topic is a kinds of message
    + Partition
        + one topic can have multiple paritions
        + each parition will be an append log file, message sent to this topic will be append to this file
        + partitions will be in different servers, this will increase the parition parallel capability
        + more paritions, can be more consumers
        + each parition can be replica to more servers (from 0.8)
        + for repplicated, there are leader and follower, leader will response to all read/write request
    + offset means the position of the message in file, it is a long
    + no random read/write in kafka, message cannot be modify after wrote to log
