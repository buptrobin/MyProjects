package com.dtise.test.tools;

import java.util.Properties;

public class KafkaTest {

	public static void main(String[] args) {
		Properties props = new Properties();

		props.put("bootstrap.servers", "localhost:9092");
		props.put("group.id", "test");
		props.put("enable.auto.commit", "true");
		props.put("auto.commit.interval.ms", "1000");
		props.put("session.timeout.ms", "30000");
		props.put("key.deserializer",
				"org.apache.kafka.common.serializa-tion.StringDeserializer");
		props.put("value.deserializer",
				"org.apache.kafka.common.serializa-tion.StringDeserializer");


	}

}
