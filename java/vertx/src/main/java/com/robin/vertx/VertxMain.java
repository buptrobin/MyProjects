package com.robin.vertx;

import io.vertx.core.Vertx;

/**
 * Created by admin on 2016/9/30.
 */
public class VertxMain {
    public static void main(String[] args){
        Vertx vertx = Vertx.vertx();

        vertx.deployVerticle(new MyVerticle(), stringAsyncResult -> {
            System.out.println("MyVerticle deployment complete");
        });

        vertx.deployVerticle(new EventBusReceiverVerticle("R1"));
        vertx.deployVerticle(new EventBusReceiverVerticle("R2"));

        try {
            Thread.sleep(2000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        vertx.deployVerticle(new EventBusSenderVerticle());
    }
}
