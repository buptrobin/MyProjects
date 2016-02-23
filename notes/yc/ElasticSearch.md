## EleasticSearch, LogStash, Kibana
### ELK Stack
![ELK Stack](https://assets.digitalocean.com/articles/elk/elk-infrastructure.png)

### ELK in one docker image
* install
```BASH
sudo docker run -p 5601:5601 -p 9200:9200 -p 5044:5044 -p 5000:5000 -it --name elk sebp/elk
```
* ports
  * 5601 (Kibana web interface).
  * 9200 (Elasticsearch JSON interface).
  * 5044 (Logstash Beats interface, receives logs from Beats such as Filebeat – see the Forwarding logs section below).
  * 5000 (Logstash Lumberjack interface, receives logs from Logstash forwarders – see the Forwarding logs section below).
![ELK Ports](http://i.imgur.com/wDertsM.png)

### Steps
+ First try
  × curl -XGET http://localhost:9200/_stats
  * /opt/logstash/bin/logstash -e 'input { stdin { } } output { elasticsearch { hosts => ["127.0.0.1"] } }'
  * curl -XPUT 'http://localhost:9200/_template/filebeat?pretty' -d@/etc/filebeat/filebeat.template.json
  * sudo /etc/init.d/filebeat start
  * sudo /etc/init.d/filebeat stop

### Linking a Docker container to the ELK container
sudo docker run -p 5601:5601 -p 9200:9200 -p 5044:5044 -p 5000:5000 -it --name elk sebp/elk
sudo docker run -p 80:80 -it --link elk:elk your/image

### Storing log data
* sudo docker run -p 5601:5601 -p 9200:9200 -p 5000:5000 -v /var/lib/elasticsearch --name elk_data sebp/elk
* sudo docker run -p 5601:5601 -p 9200:9200 -p 5000:5000 --volumes-from elk_data --name elk sebp/elk
