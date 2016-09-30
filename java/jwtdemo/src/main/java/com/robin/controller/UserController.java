package com.robin.controller;

/**
 * Created by robin on 9/26/16.
 */
import java.util.Arrays;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.ServletException;

import com.robin.jwt.JwtHelper;
import org.springframework.web.bind.annotation.*;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;

@RestController
@RequestMapping("/user")
public class UserController {

    private final Map<String, List<String>> userDb = new HashMap<>();
    private final Map<String, String> tokenCache = new HashMap<>();

    public UserController() {
        userDb.put("tom", Arrays.asList("user"));
        userDb.put("sally", Arrays.asList("user", "admin"));
    }

    @RequestMapping(value = "login", method = RequestMethod.POST)
    public LoginResponse login(@RequestBody final UserLogin login)
            throws ServletException {
        if (login.name == null || !userDb.containsKey(login.name) || !login.password.equals("111")) {
            throw new ServletException("Invalid login");
        }

        String token = Jwts.builder().setSubject(login.name)
                .claim("roles", userDb.get(login.name))
                .setIssuedAt(new Date())
                .signWith(SignatureAlgorithm.HS256, JwtHelper.SIGN_KEY)
                .compact();

        return new LoginResponse(token);
    }

    @GetMapping(value="greeting")
    String greeting() {
        return "greeting!";
    }

    @SuppressWarnings("unused")
    private static class UserLogin {
        public String name;
        public String password;
    }

    @SuppressWarnings("unused")
    private static class LoginResponse {
        public String token;

        public LoginResponse(final String token) {
            this.token = token;
        }
    }
}