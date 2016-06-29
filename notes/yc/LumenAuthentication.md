## 5.1 Session
### Note
+ Lumen 5.2 对 Lumen 进行了大瘦身专注于，并且将专注提供无状态的 JSON API 服务。因此，Lumen 框架将不再包含Session和视图（View）。如果你需要使用这些功能，请用完整的 Laravel 框架。将 Lumen 应用升级到完整的 Laravel 应用只需：将你自己开发的代码拷贝到全新安装的 Laravel 项目中.
由于 session 不再包含在 Lumen 中，因此，认证（authentication）必须通过 API token 或者请求头来实现无状态化。在全新实现的 AuthServiceProvider 中你对认证过程拥有完全的控制权。

+ Create project
  > composer create-project laravel/lumen blog "5.1.*"

### Install
+ Install Lumen
  > composer global require "laravel/lumen-installer=~1.0"
  > set PATH=$PATH:$HOME/.composer/vendor/bin

+ Run Project
  > robin@robin-dev:~/auth$ php artisan serve 开启简单的调试服务器
## Solution of https://github.com/thebhandariprakash/LumenAuthentication
1. unzip
2. copy vendor folder to it
3. When meet error of key, modify the the .env
  > APP_KEY=xlhF31NeOlibJcoOW9tvZg7TkHcAZI3a
4. When meet error of cannot find memcached
  > CACHE_DRIVER=array
    SESSION_DRIVER=array
    QUEUE_DRIVER=array

5. When meet error: ErrorException in AuthManager.php line 16:
  + Change config/auth.php
    > return [
    'driver' => 'eloquent',
    'model' => App\User::class,
    'table' => 'users',
## Solution of LumenAuthen (5.2, token)
+ http://www.thatyou.cn/lumen%E5%AE%9E%E7%8E%B0%E7%94%A8%E6%88%B7%E6%B3%A8%E5%86%8C%E7%99%BB%E5%BD%95%E8%AE%A4%E8%AF%81/

## Solution of custom Middleware

## Solution of https://github.com/stackphp/session

## Solution of using OAUTH
### Reference
+ http://wenda.golaravel.com/question/1303
+ https://segmentfault.com/a/1190000002902055
+ https://stormpath.com/blog/5-minute-authentication-with-lumen-and-stormpath
+ https://github.com/thebhandariprakash/LumenAuthentication
+ Lumen的认证系统  https://github.com/Lee2011/lumen-angular/wiki/Lumen%E7%9A%84%E8%AE%A4%E8%AF%81%E7%B3%BB%E7%BB%9F
