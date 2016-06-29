## Laravel
+ Install Laravel http://tecadmin.net/install-laravel-framework-on-ubuntu/
  > sudo composer global require "laravel/installer"
  > apt-get install mysql-server php5-mysql
  > sudo apt-get install -y php5 php5-mcrypt php5-gd

+ use composer install --prefer-dist
  1. Download https://github.com/scotch-io/laravel-angular-comment-app
  1. change directory: cd laravel-angular-comment-app/
  1. Install Laravel: sudo composer install --prefer-dist
    + For error "Mcrypt PHP extension required."
      > sudo apt-get install Mcrypt

      ```
      cd /etc/php5/mods-available
      ln -sf ../conf.d/mcrypt.ini .
      php5enmod mcrypt
      service nginx reload
      ```
  1. Change your database settings in app/config/database.php
  1. Migrate your database: php artisan migrate
    + ERROR: Could not find driver
      ```
      sudo apt-get php5-mysql
      /etc/init.d/php5-fpm restart
      ```
  1. Seed your database: php artisan db:seed
  1. View your application in browser.
+ Nginx Configuration
  > /etc/nginx/sites-enabled the default configuration

### Authentication
+ Reference
  > http://www.tuicool.com/articles/3y6FRnz Laravel 5.2 新特性系列 —— 多用户认证功能实现详解
1. php artisan make:auth 该Artisan命令会生成用户认证所需的路由、视图以及HomeController, 更新routes.php
2. php artisan migrate, will generate two tables for login
1. http://laravelacademy.org/post/1258.html Laravel 5.1用户认证
1. http://laravelacademy.org/post/163.html#ipt_kb_toc_163_9 [ Laravel 5.1 文档 ] 服务 —— 用户认证
### Model
+ Create model
  ```
  php artisan make:model Article
  php artisan make:model Page
  ```
### New Blog Project http://www.zhangxihai.cn/archives/120
+ install lavarel
> composer create-project laravel/laravel . --prefer-dist
+ create new project
> laravel new myblog
+ Run web server
> php artisan serve
+ create models
> php artisan make:model --migration Post
> or php artisan make:model --migration Models\Post, then Post model will be in Models directory
+ Change the database\migrations\xx_create_posts_table.php
> up() is to create table schema
+ Run migration to create table
> php artisan migrate
+ change Post.php
+ database/factories/ModelFactory.php, define how to generate test data
+ seed the database
> php artisan db:seed
+ create controller
> php artisan make:controller BlogController --plain



### Reference
+ https://phphub.org/topics/533 Laravel 5 系列入门教程
+ https://scotch.io/tutorials/build-a-time-tracker-with-laravel-5-and-angularjs-part-1 Build a Time Tracker with Laravel 5 and AngularJS
+ https://www.codetutorial.io/laravel-5-angularjs-tutorial/ Laravel 5 and AngularJS Tutorial
