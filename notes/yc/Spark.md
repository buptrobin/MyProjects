 
## Spark
+ What's Spark
    * a bid data processing framework, manage different data and data source
    * 
+ Arcitecture

    ![architecture](https://2.bp.blogspot.com/-7-VC7xgjzsU/VUzrtuf1yGI/AAAAAAAAAJI/SAnEKH27fZU/s640/kafka.PNG)

+ Producer

    ![Kafka cluster producer](http://www.linuxidc.com/upload/2014_09/140929195786745.jpg)

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

            - **request.required.acks**, whether want the producer to require an acknowledgement from broker when message received
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
    + one cusumer belong to on consumer group, but one msg will only send to one consumer.
    + 我们可以认为一个group是一个"订阅"者,一个Topic中的每个partions,只会被一个"订阅者"中的一个consumer消费, 不过一个consumer可以同时消费多个partitions中的消息
    + kafka只能保证一个partition中的消息被某个consumer消费时是顺序的.事实上,从Topic角度来说,当有多个partitions时,消息仍不是全局有序的.
    + 对于一个topic,同一个group中不能有多于partitions个数的consumer同时消费,否则将意味着某些consumer将无法得到消息.
        
    That means, messages from one partition will be sent to different consumer in one consumer group, which make it **`load balance`**.

    ![Kafka Cluster](http://kafka.apache.org/images/consumer-groups.png)

+ Kafka cluster
    + one kafka instance means broker
    + zookeeper to ensure cluster/producer/consumer system availability and meta info
+ Topic is a kinds of message

    ![Topic](http://sookocheff.com/img/kafka/kafka-in-a-nutshell/log-anatomy.png)

    + Partition
        + one topic can have multiple paritions
        + each parition will be an append log file, message sent to this topic will be append to this file
        + partitions will be in different servers, this will increase the parition parallel capability
        + more paritions, can be more consumers
        + each parition can be replica to more servers (from 0.8)
        + for repplicated, there are leader and follower, leader will response to all read/write request
    + offset means the position of the message in file, it is a long
    + no random read/write in kafka, message cannot be modify after wrote to log
+ Motivion
    * 


### Try and Test
+ ~/kafka/bin/kafka-server-start.sh ~/kafka/config/server.properties // Start the broker
+ ~/kafka/bin/kafka-console-producer.sh --broker-list localhost:9092 --topic TutorialTopic // Start the producer
+ ~/kafka/bin/kafka-console-consumer.sh --zookeeper localhost:2181 --topic TutorialTopic --from-benning // Start the consumer
+ About topic 
    +   ~/kafka/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 3 --partitions 1 --topic my-replicated-topic // create a new topic with a replication factor of three
    + ~/kafka/bin/kafka-topics.sh --describe --zookeeper localhost:2181 --topic my-replicated-topic
    + bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic test
    + bin/kafka-topics.sh --list --zookeeper localhost:2181



