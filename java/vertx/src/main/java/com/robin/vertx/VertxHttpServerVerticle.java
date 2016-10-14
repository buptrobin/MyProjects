package com.robin.vertx;

import io.vertx.core.AbstractVerticle;
import io.vertx.core.Handler;
import io.vertx.core.http.HttpServer;
import io.vertx.core.http.HttpServerRequest;
import io.vertx.core.http.HttpServerResponse;

/**
 * Created by robin on 9/30/16.
 */
public class VertxHttpServerVerticle extends AbstractVerticle {
    private HttpServer httpServer = null;

    @Override
    public void start() throws Exception {
        System.out.println("VertHttpServerVerticle started!");

        httpServer = vertx.createHttpServer();

        httpServer.requestHandler( new Handler<HttpServerRequest>() {
            @Override
            public void handle(HttpServerRequest request) {
                System.out.println("incoming request!");

                HttpServerResponse response = request.response();
                response.setStatusCode(200);
                response.headers()
                        .add("Content-Length", String.valueOf(57))
                        .add("Content-Type", "text/html");

                response.write("Vert.x is alive!");
                response.end();
            }
        });
        httpServer.listen(9999);
    }

    @Override
    public void stop() throws Exception {
        httpServer.close();
    }
}
