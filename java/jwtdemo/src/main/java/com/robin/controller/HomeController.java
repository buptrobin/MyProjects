package com.robin.controller;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import javax.xml.ws.Response;

/**
 * Created by robin on 9/26/16.
 */

@RestController
public class HomeController {

    @GetMapping(value="/hello")
    public String hello(){
        return "Hello";
    }


}
