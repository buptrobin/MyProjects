package com.robin.vertx;

import io.vertx.core.AbstractVerticle;

/**
 * Created by admin on 2016/9/30.
 */
public class EventBusSenderVerticle extends AbstractVerticle {
    @Override
    public void start() throws Exception {
        vertx.eventBus().publish("anAddress","msg 2");
        vertx.eventBus().send("anAddress", "msg 1");
    }
}
