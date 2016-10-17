package com.robin.jwt;

import com.robin.filter.AuthFilter;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.servlet.FilterRegistrationBean;
import org.springframework.context.annotation.Bean;

@SpringBootApplication(scanBasePackages = { "com.robin.controller"})
@EnableAutoConfiguration
public class JwtdemoApplication {

    @Bean
    public FilterRegistrationBean jwtFilter() {
        final FilterRegistrationBean registrationBean = new FilterRegistrationBean();
        registrationBean.setFilter(new AuthFilter());
        registrationBean.addUrlPatterns("/api/*");

        return registrationBean;
    }

	public static void main(String[] args) {
		SpringApplication.run(JwtdemoApplication.class, args);
	}
}
