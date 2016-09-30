package com.robin.vertx;

import io.vertx.core.AbstractVerticle;
import io.vertx.core.Future;

/**
 * Created by admin on 2016/9/30.
 */
public class EventBusReceiverVerticle extends AbstractVerticle {
    private String name = "";

    public EventBusReceiverVerticle(String name){
        this.name = name;
    }

    @Override
    public void start(Future<Void> startFuture) throws Exception {
        vertx.eventBus().consumer("anAddress", message -> {
            System.out.println( name+": 1 received message.body() = "
                    + message.body());
        });
    }
}
