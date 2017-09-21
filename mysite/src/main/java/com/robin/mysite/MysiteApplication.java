package com.robin.mysite;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.jdbc.DataSourceAutoConfiguration;
import org.springframework.boot.autoconfigure.orm.jpa.HibernateJpaAutoConfiguration;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.ArrayList;
import java.util.List;

@Controller
@SpringBootApplication(exclude = {DataSourceAutoConfiguration.class, HibernateJpaAutoConfiguration.class})
public class MysiteApplication {
    public static void main(String[] args) {
        SpringApplication.run(MysiteApplication.class, args);
    }

    @RequestMapping("/")
    public String index(Model model) {
        Person single = new Person("aa", 1);
        List<Person> people = new ArrayList<Person>();
        Person p1 = new Person("bb", 2);
        Person p2 = new Person("cc", 3);
        Person p3 = new Person("dd", 4);
        people.add(p1);
        people.add(p2);
        people.add(p3);
        model.addAttribute("singlePerson", single);
        model.addAttribute("people", people);
        return "index";
    }
}
