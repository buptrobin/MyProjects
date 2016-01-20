



<!DOCTYPE html>
<html lang="en" class=" is-copy-enabled is-u2f-enabled">
  <head prefix="og: http://ogp.me/ns# fb: http://ogp.me/ns/fb# object: http://ogp.me/ns/object# article: http://ogp.me/ns/article# profile: http://ogp.me/ns/profile#">
    <meta charset='utf-8'>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Language" content="en">
    <meta name="viewport" content="width=1020">
    
    
    <title>README/README.md at master · guodongxiaren/README</title>
    <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="GitHub">
    <link rel="fluid-icon" href="https://github.com/fluidicon.png" title="GitHub">
    <link rel="apple-touch-icon" sizes="57x57" href="/apple-touch-icon-114.png">
    <link rel="apple-touch-icon" sizes="114x114" href="/apple-touch-icon-114.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/apple-touch-icon-144.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/apple-touch-icon-144.png">
    <meta property="fb:app_id" content="1401488693436528">

      <meta content="@github" name="twitter:site" /><meta content="summary" name="twitter:card" /><meta content="guodongxiaren/README" name="twitter:title" /><meta content="README文件语法解读，即Github Flavored Markdown语法介绍" name="twitter:description" /><meta content="https://avatars1.githubusercontent.com/u/5945107?v=3&amp;s=400" name="twitter:image:src" />
      <meta content="GitHub" property="og:site_name" /><meta content="object" property="og:type" /><meta content="https://avatars1.githubusercontent.com/u/5945107?v=3&amp;s=400" property="og:image" /><meta content="guodongxiaren/README" property="og:title" /><meta content="https://github.com/guodongxiaren/README" property="og:url" /><meta content="README文件语法解读，即Github Flavored Markdown语法介绍" property="og:description" />
      <meta name="browser-stats-url" content="https://api.github.com/_private/browser/stats">
    <meta name="browser-errors-url" content="https://api.github.com/_private/browser/errors">
    <link rel="assets" href="https://assets-cdn.github.com/">
    <link rel="web-socket" href="wss://live.github.com/_sockets/MTY0ODU4OmIzYTc3ZGMxNjUyODAzYjQzYzk3MGNhZDRmZGJhYjk4OmQwN2JhMzkzZTU0ZDVmZmU1YTE5MzYwYzlmN2NjY2U0YjgxNjcyNjI5NzE3NTQyOTZjZjAxODE5ZjMzMWJlNjI=--cc60a4c7fca56c0203f69b6ae76ec76c9b44d0e3">
    <meta name="pjax-timeout" content="1000">
    <link rel="sudo-modal" href="/sessions/sudo_modal">

    <meta name="msapplication-TileImage" content="/windows-tile.png">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="selected-link" value="repo_source" data-pjax-transient>

    <meta name="google-site-verification" content="KT5gs8h0wvaagLKAVWq8bbeNwnZZK1r1XQysX3xurLU">
    <meta name="google-analytics" content="UA-3769691-2">

<meta content="collector.githubapp.com" name="octolytics-host" /><meta content="github" name="octolytics-app-id" /><meta content="D20D5C72:7219:7C1997:569F4A17" name="octolytics-dimension-request_id" /><meta content="164858" name="octolytics-actor-id" /><meta content="buptrobin" name="octolytics-actor-login" /><meta content="d9ae1e67db99328b4730fed82a707cff3a0604ff22c31eb059e3ac9c03bb66e9" name="octolytics-actor-hash" />
<meta content="/&lt;user-name&gt;/&lt;repo-name&gt;/blob/show" data-pjax-transient="true" name="analytics-location" />
<meta content="Rails, view, blob#show" data-pjax-transient="true" name="analytics-event" />


  <meta class="js-ga-set" name="dimension1" content="Logged In">



        <meta name="hostname" content="github.com">
    <meta name="user-login" content="buptrobin">

        <meta name="expected-hostname" content="github.com">

      <link rel="mask-icon" href="https://assets-cdn.github.com/pinned-octocat.svg" color="#4078c0">
      <link rel="icon" type="image/x-icon" href="https://assets-cdn.github.com/favicon.ico">

    <meta content="5a855038ea7e99088a03f7fecf89f8d9af74a438" name="form-nonce" />

    <link crossorigin="anonymous" href="https://assets-cdn.github.com/assets/github-e64d783fc73cc815bb639b1ee740d83c08b1a72e2955dbd871b5971946f6f73d.css" integrity="sha256-5k14P8c8yBW7Y5se50DYPAixpy4pVdvYcbWXGUb29z0=" media="all" rel="stylesheet" />
    <link crossorigin="anonymous" href="https://assets-cdn.github.com/assets/github2-d67e665a5adcb4911576562cbeb82c00d697b1f31e846c253fec048877a6b457.css" integrity="sha256-1n5mWlrctJEVdlYsvrgsANaXsfMehGwlP+wEiHemtFc=" media="all" rel="stylesheet" />
    
    


    <meta http-equiv="x-pjax-version" content="36663430747823914febb0e91e3b8b2f">

      
  <meta name="description" content="README文件语法解读，即Github Flavored Markdown语法介绍">
  <meta name="go-import" content="github.com/guodongxiaren/README git https://github.com/guodongxiaren/README.git">

  <meta content="5945107" name="octolytics-dimension-user_id" /><meta content="guodongxiaren" name="octolytics-dimension-user_login" /><meta content="20849413" name="octolytics-dimension-repository_id" /><meta content="guodongxiaren/README" name="octolytics-dimension-repository_nwo" /><meta content="true" name="octolytics-dimension-repository_public" /><meta content="false" name="octolytics-dimension-repository_is_fork" /><meta content="20849413" name="octolytics-dimension-repository_network_root_id" /><meta content="guodongxiaren/README" name="octolytics-dimension-repository_network_root_nwo" />
  <link href="https://github.com/guodongxiaren/README/commits/master.atom" rel="alternate" title="Recent Commits to README:master" type="application/atom+xml">

  </head>


  <body class="logged_in   env-production linux vis-public page-blob">
    <a href="#start-of-content" tabindex="1" class="accessibility-aid js-skip-to-content">Skip to content</a>

    
    
    



      <div class="header header-logged-in true" role="banner">
  <div class="container clearfix">

    <a class="header-logo-invertocat" href="https://github.com/" data-hotkey="g d" aria-label="Homepage" data-ga-click="Header, go to dashboard, icon:logo">
  <span aria-hidden="true" class="mega-octicon octicon-mark-github"></span>
</a>


      <div class="site-search repo-scope js-site-search" role="search">
          <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/guodongxiaren/README/search" class="js-site-search-form" data-global-search-url="/search" data-repo-search-url="/guodongxiaren/README/search" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
  <label class="js-chromeless-input-container form-control">
    <div class="scope-badge">This repository</div>
    <input type="text"
      class="js-site-search-focus js-site-search-field is-clearable chromeless-input"
      data-hotkey="s"
      name="q"
      placeholder="Search"
      aria-label="Search this repository"
      data-global-scope-placeholder="Search GitHub"
      data-repo-scope-placeholder="Search"
      tabindex="1"
      autocapitalize="off">
  </label>
</form>
      </div>

      <ul class="header-nav left" role="navigation">
        <li class="header-nav-item">
          <a href="/pulls" class="js-selected-navigation-item header-nav-link" data-ga-click="Header, click, Nav menu - item:pulls context:user" data-hotkey="g p" data-selected-links="/pulls /pulls/assigned /pulls/mentioned /pulls">
            Pull requests
</a>        </li>
        <li class="header-nav-item">
          <a href="/issues" class="js-selected-navigation-item header-nav-link" data-ga-click="Header, click, Nav menu - item:issues context:user" data-hotkey="g i" data-selected-links="/issues /issues/assigned /issues/mentioned /issues">
            Issues
</a>        </li>
          <li class="header-nav-item">
            <a class="header-nav-link" href="https://gist.github.com/" data-ga-click="Header, go to gist, text:gist">Gist</a>
          </li>
      </ul>

    
<ul class="header-nav user-nav right" id="user-links">
  <li class="header-nav-item">
      <span class="js-socket-channel js-updatable-content"
        data-channel="notification-changed:buptrobin"
        data-url="/notifications/header">
      <a href="/notifications" aria-label="You have no unread notifications" class="header-nav-link notification-indicator tooltipped tooltipped-s" data-ga-click="Header, go to notifications, icon:read" data-hotkey="g n">
          <span class="mail-status all-read"></span>
          <span aria-hidden="true" class="octicon octicon-bell"></span>
</a>  </span>

  </li>

  <li class="header-nav-item dropdown js-menu-container">
    <a class="header-nav-link tooltipped tooltipped-s js-menu-target" href="/new"
       aria-label="Create new…"
       data-ga-click="Header, create new, icon:add">
      <span aria-hidden="true" class="octicon octicon-plus left"></span>
      <span class="dropdown-caret"></span>
    </a>

    <div class="dropdown-menu-content js-menu-content">
      <ul class="dropdown-menu dropdown-menu-sw">
        
<a class="dropdown-item" href="/new" data-ga-click="Header, create new repository">
  New repository
</a>


  <a class="dropdown-item" href="/organizations/new" data-ga-click="Header, create new organization">
    New organization
  </a>



  <div class="dropdown-divider"></div>
  <div class="dropdown-header">
    <span title="guodongxiaren/README">This repository</span>
  </div>
    <a class="dropdown-item" href="/guodongxiaren/README/issues/new" data-ga-click="Header, create new issue">
      New issue
    </a>

      </ul>
    </div>
  </li>

  <li class="header-nav-item dropdown js-menu-container">
    <a class="header-nav-link name tooltipped tooltipped-sw js-menu-target" href="/buptrobin"
       aria-label="View profile and more"
       data-ga-click="Header, show menu, icon:avatar">
      <img alt="@buptrobin" class="avatar" height="20" src="https://avatars0.githubusercontent.com/u/164858?v=3&amp;s=40" width="20" />
      <span class="dropdown-caret"></span>
    </a>

    <div class="dropdown-menu-content js-menu-content">
      <div class="dropdown-menu  dropdown-menu-sw">
        <div class=" dropdown-header header-nav-current-user css-truncate">
            Signed in as <strong class="css-truncate-target">buptrobin</strong>

        </div>


        <div class="dropdown-divider"></div>

          <a class="dropdown-item" href="/buptrobin" data-ga-click="Header, go to profile, text:your profile">
            Your profile
          </a>
        <a class="dropdown-item" href="/stars" data-ga-click="Header, go to starred repos, text:your stars">
          Your stars
        </a>
        <a class="dropdown-item" href="/explore" data-ga-click="Header, go to explore, text:explore">
          Explore
        </a>
          <a class="dropdown-item" href="/integrations" data-ga-click="Header, go to integrations, text:integrations">
            Integrations
          </a>
        <a class="dropdown-item" href="https://help.github.com" data-ga-click="Header, go to help, text:help">
          Help
        </a>

          <div class="dropdown-divider"></div>

          <a class="dropdown-item" href="/settings/profile" data-ga-click="Header, go to settings, icon:settings">
            Settings
          </a>

          <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/logout" class="logout-form" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="8U9g7ua/uy0AlQnmbRLU8ligA3/X1L+bmEO3xNqMnF8JYcDJ4pYQHM1ETPlAI5rAZ45suqOGeR+rnuqQYsazRg==" /></div>
            <button class="dropdown-item dropdown-signout" data-ga-click="Header, sign out, icon:logout">
              Sign out
            </button>
</form>
      </div>
    </div>
  </li>
</ul>


    
  </div>
</div>

      

      


    <div id="start-of-content" class="accessibility-aid"></div>

      <div id="js-flash-container">
</div>


    <div role="main" class="main-content">
        <div itemscope itemtype="http://schema.org/WebPage">
    <div id="js-repo-pjax-container" class="context-loader-container js-repo-nav-next" data-pjax-container>
      
<div class="pagehead repohead instapaper_ignore readability-menu experiment-repo-nav">
  <div class="container repohead-details-container">

    

<ul class="pagehead-actions">

  <li>
        <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/notifications/subscribe" class="js-social-container" data-autosubmit="true" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="sEJcMf/HmY1VBA+8m6WSAe/r9NZesJ0kdx+XlPMEg9wHmssO0siHcjg3CJtPi0NKCEFduTDQti4WTMeB/WpHBA==" /></div>      <input id="repository_id" name="repository_id" type="hidden" value="20849413" />

        <div class="select-menu js-menu-container js-select-menu">
          <a href="/guodongxiaren/README/subscription"
            class="btn btn-sm btn-with-count select-menu-button js-menu-target" role="button" tabindex="0" aria-haspopup="true"
            data-ga-click="Repository, click Watch settings, action:blob#show">
            <span class="js-select-button">
              <span aria-hidden="true" class="octicon octicon-eye"></span>
              Watch
            </span>
          </a>
          <a class="social-count js-social-count" href="/guodongxiaren/README/watchers">
            23
          </a>

        <div class="select-menu-modal-holder">
          <div class="select-menu-modal subscription-menu-modal js-menu-content" aria-hidden="true">
            <div class="select-menu-header">
              <span aria-label="Close" class="octicon octicon-x js-menu-close" role="button"></span>
              <span class="select-menu-title">Notifications</span>
            </div>

              <div class="select-menu-list js-navigation-container" role="menu">

                <div class="select-menu-item js-navigation-item selected" role="menuitem" tabindex="0">
                  <span aria-hidden="true" class="octicon octicon-check select-menu-item-icon"></span>
                  <div class="select-menu-item-text">
                    <input checked="checked" id="do_included" name="do" type="radio" value="included" />
                    <span class="select-menu-item-heading">Not watching</span>
                    <span class="description">Be notified when participating or @mentioned.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <span aria-hidden="true" class="octicon octicon-eye"></span>
                      Watch
                    </span>
                  </div>
                </div>

                <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
                  <span aria-hidden="true" class="octicon octicon-check select-menu-item-icon"></span>
                  <div class="select-menu-item-text">
                    <input id="do_subscribed" name="do" type="radio" value="subscribed" />
                    <span class="select-menu-item-heading">Watching</span>
                    <span class="description">Be notified of all conversations.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <span aria-hidden="true" class="octicon octicon-eye"></span>
                      Unwatch
                    </span>
                  </div>
                </div>

                <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
                  <span aria-hidden="true" class="octicon octicon-check select-menu-item-icon"></span>
                  <div class="select-menu-item-text">
                    <input id="do_ignore" name="do" type="radio" value="ignore" />
                    <span class="select-menu-item-heading">Ignoring</span>
                    <span class="description">Never be notified.</span>
                    <span class="js-select-button-text hidden-select-button-text">
                      <span aria-hidden="true" class="octicon octicon-mute"></span>
                      Stop ignoring
                    </span>
                  </div>
                </div>

              </div>

            </div>
          </div>
        </div>
</form>
  </li>

  <li>
    
  <div class="js-toggler-container js-social-container starring-container ">

    <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/guodongxiaren/README/unstar" class="js-toggler-form starred js-unstar-button" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="RHHvGyXakkI4uKjDgfGaqzbs+BMw99m8mIqtCwYDGyahn70/8v4IBl8PNteIs/xpOC1AsMXhqh56rpPxBd5KCg==" /></div>
      <button
        class="btn btn-sm btn-with-count js-toggler-target"
        aria-label="Unstar this repository" title="Unstar guodongxiaren/README"
        data-ga-click="Repository, click unstar button, action:blob#show; text:Unstar">
        <span aria-hidden="true" class="octicon octicon-star"></span>
        Unstar
      </button>
        <a class="social-count js-social-count" href="/guodongxiaren/README/stargazers">
          227
        </a>
</form>
    <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/guodongxiaren/README/star" class="js-toggler-form unstarred js-star-button" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="KZ6AxroPLqBhgG6qXTk3YL9VJ7jTFG3eEhUPCChn/vfqWixHmCdWQljBtZ8N70Dtco/WwWmr3Ofw4sHlnLhPhg==" /></div>
      <button
        class="btn btn-sm btn-with-count js-toggler-target"
        aria-label="Star this repository" title="Star guodongxiaren/README"
        data-ga-click="Repository, click star button, action:blob#show; text:Star">
        <span aria-hidden="true" class="octicon octicon-star"></span>
        Star
      </button>
        <a class="social-count js-social-count" href="/guodongxiaren/README/stargazers">
          227
        </a>
</form>  </div>

  </li>

  <li>
          <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/guodongxiaren/README/fork" class="btn-with-count" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="KgVxqGQxgg9tyxHwF3VXp97zMJCvjo9F3ugqxCA+tBbp9ZIiUhWnuA1AfZW8D/FB/lgFh4H3SwxhJkrN4NwaRQ==" /></div>
            <button
                type="submit"
                class="btn btn-sm btn-with-count"
                data-ga-click="Repository, show fork modal, action:blob#show; text:Fork"
                title="Fork your own copy of guodongxiaren/README to your account"
                aria-label="Fork your own copy of guodongxiaren/README to your account">
              <span aria-hidden="true" class="octicon octicon-repo-forked"></span>
              Fork
            </button>
</form>
    <a href="/guodongxiaren/README/network" class="social-count">
      490
    </a>
  </li>
</ul>

    <h1 itemscope itemtype="http://data-vocabulary.org/Breadcrumb" class="entry-title public ">
  <span aria-hidden="true" class="octicon octicon-repo"></span>
  <span class="author"><a href="/guodongxiaren" class="url fn" itemprop="url" rel="author"><span itemprop="title">guodongxiaren</span></a></span><!--
--><span class="path-divider">/</span><!--
--><strong><a href="/guodongxiaren/README" data-pjax="#js-repo-pjax-container">README</a></strong>

  <span class="page-context-loader">
    <img alt="" height="16" src="https://assets-cdn.github.com/images/spinners/octocat-spinner-32.gif" width="16" />
  </span>

</h1>

  </div>
  <div class="container">
    
<nav class="reponav js-repo-nav js-sidenav-container-pjax js-octicon-loaders"
     role="navigation"
     data-pjax="#js-repo-pjax-container">

  <a href="/guodongxiaren/README" aria-label="Code" aria-selected="true" class="js-selected-navigation-item selected reponav-item" data-hotkey="g c" data-selected-links="repo_source repo_downloads repo_commits repo_releases repo_tags repo_branches /guodongxiaren/README">
    <span aria-hidden="true" class="octicon octicon-code"></span>
    Code
</a>
    <a href="/guodongxiaren/README/issues" class="js-selected-navigation-item reponav-item" data-hotkey="g i" data-selected-links="repo_issues repo_labels repo_milestones /guodongxiaren/README/issues">
      <span aria-hidden="true" class="octicon octicon-issue-opened"></span>
      Issues
      <span class="counter">0</span>
</a>
  <a href="/guodongxiaren/README/pulls" class="js-selected-navigation-item reponav-item" data-hotkey="g p" data-selected-links="repo_pulls /guodongxiaren/README/pulls">
    <span aria-hidden="true" class="octicon octicon-git-pull-request"></span>
    Pull requests
    <span class="counter">0</span>
</a>
    <a href="/guodongxiaren/README/wiki" class="js-selected-navigation-item reponav-item" data-hotkey="g w" data-selected-links="repo_wiki /guodongxiaren/README/wiki">
      <span aria-hidden="true" class="octicon octicon-book"></span>
      Wiki
</a>
  <a href="/guodongxiaren/README/pulse" class="js-selected-navigation-item reponav-item" data-selected-links="pulse /guodongxiaren/README/pulse">
    <span aria-hidden="true" class="octicon octicon-pulse"></span>
    Pulse
</a>
  <a href="/guodongxiaren/README/graphs" class="js-selected-navigation-item reponav-item" data-selected-links="repo_graphs repo_contributors /guodongxiaren/README/graphs">
    <span aria-hidden="true" class="octicon octicon-graph"></span>
    Graphs
</a>

</nav>

  </div>
</div>

<div class="container new-discussion-timeline experiment-repo-nav">
  <div class="repository-content">

    

<a href="/guodongxiaren/README/blob/df9232bb893abb0d4f49715ef18851ae6048eba8/README.md" class="hidden js-permalink-shortcut" data-hotkey="y">Permalink</a>

<!-- blob contrib key: blob_contributors:v21:69d5cb960da1b92b5000b15ab6d28627 -->

<div class="file-navigation js-zeroclipboard-container">
  
<div class="select-menu js-menu-container js-select-menu left">
  <button class="btn btn-sm select-menu-button js-menu-target css-truncate" data-hotkey="w"
    title="master"
    type="button" aria-label="Switch branches or tags" tabindex="0" aria-haspopup="true">
    <i>Branch:</i>
    <span class="js-select-button css-truncate-target">master</span>
  </button>

  <div class="select-menu-modal-holder js-menu-content js-navigation-container" data-pjax aria-hidden="true">

    <div class="select-menu-modal">
      <div class="select-menu-header">
        <span aria-label="Close" class="octicon octicon-x js-menu-close" role="button"></span>
        <span class="select-menu-title">Switch branches/tags</span>
      </div>

      <div class="select-menu-filters">
        <div class="select-menu-text-filter">
          <input type="text" aria-label="Filter branches/tags" id="context-commitish-filter-field" class="js-filterable-field js-navigation-enable" placeholder="Filter branches/tags">
        </div>
        <div class="select-menu-tabs">
          <ul>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="branches" data-filter-placeholder="Filter branches/tags" class="js-select-menu-tab" role="tab">Branches</a>
            </li>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="tags" data-filter-placeholder="Find a tag…" class="js-select-menu-tab" role="tab">Tags</a>
            </li>
          </ul>
        </div>
      </div>

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="branches" role="menu">

        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <a class="select-menu-item js-navigation-item js-navigation-open selected"
               href="/guodongxiaren/README/blob/master/README.md"
               data-name="master"
               data-skip-pjax="true"
               rel="nofollow">
              <span aria-hidden="true" class="octicon octicon-check select-menu-item-icon"></span>
              <span class="select-menu-item-text css-truncate-target" title="master">
                master
              </span>
            </a>
        </div>

          <div class="select-menu-no-results">Nothing to show</div>
      </div>

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="tags">
        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <a class="select-menu-item js-navigation-item js-navigation-open "
              href="/guodongxiaren/README/tree/One/README.md"
              data-name="One"
              data-skip-pjax="true"
              rel="nofollow">
              <span aria-hidden="true" class="octicon octicon-check select-menu-item-icon"></span>
              <span class="select-menu-item-text css-truncate-target" title="One">
                One
              </span>
            </a>
        </div>

        <div class="select-menu-no-results">Nothing to show</div>
      </div>

    </div>
  </div>
</div>

  <div class="btn-group right">
    <a href="/guodongxiaren/README/find/master"
          class="js-show-file-finder btn btn-sm"
          data-pjax
          data-hotkey="t">
      Find file
    </a>
    <button aria-label="Copy file path to clipboard" class="js-zeroclipboard btn btn-sm zeroclipboard-button tooltipped tooltipped-s" data-copied-hint="Copied!" type="button">Copy path</button>
  </div>
  <div class="breadcrumb js-zeroclipboard-target">
    <span class="repo-root js-repo-root"><span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/guodongxiaren/README" class="" data-branch="master" data-pjax="true" itemscope="url"><span itemprop="title">README</span></a></span></span><span class="separator">/</span><strong class="final-path">README.md</strong>
  </div>
</div>


  <div class="commit-tease">
      <span class="right">
        <a class="commit-tease-sha" href="/guodongxiaren/README/commit/cb865d519fa47d2d118313e194b422285130618f" data-pjax>
          cb865d5
        </a>
        <time datetime="2015-11-10T08:01:36Z" is="relative-time">Nov 10, 2015</time>
      </span>
      <div>
        <img alt="@jiexishede" class="avatar" height="20" src="https://avatars2.githubusercontent.com/u/9484734?v=3&amp;s=40" width="20" />
        <a href="/jiexishede" class="user-mention" rel="contributor">jiexishede</a>
          <a href="/guodongxiaren/README/commit/cb865d519fa47d2d118313e194b422285130618f" class="message" data-pjax="true" title="修改图片超链的表达方式">修改图片超链的表达方式</a>
      </div>

    <div class="commit-tease-contributors">
      <a class="muted-link contributors-toggle" href="#blob_contributors_box" rel="facebox">
        <strong>4</strong>
         contributors
      </a>
          <a class="avatar-link tooltipped tooltipped-s" aria-label="guodongxiaren" href="/guodongxiaren/README/commits/master/README.md?author=guodongxiaren"><img alt="@guodongxiaren" class="avatar" height="20" src="https://avatars0.githubusercontent.com/u/5945107?v=3&amp;s=40" width="20" /> </a>
    <a class="avatar-link tooltipped tooltipped-s" aria-label="jiexishede" href="/guodongxiaren/README/commits/master/README.md?author=jiexishede"><img alt="@jiexishede" class="avatar" height="20" src="https://avatars2.githubusercontent.com/u/9484734?v=3&amp;s=40" width="20" /> </a>
    <a class="avatar-link tooltipped tooltipped-s" aria-label="XinHuaLuFang" href="/guodongxiaren/README/commits/master/README.md?author=XinHuaLuFang"><img alt="@XinHuaLuFang" class="avatar" height="20" src="https://avatars3.githubusercontent.com/u/7486847?v=3&amp;s=40" width="20" /> </a>
    <a class="avatar-link tooltipped tooltipped-s" aria-label="88250" href="/guodongxiaren/README/commits/master/README.md?author=88250"><img alt="@88250" class="avatar" height="20" src="https://avatars2.githubusercontent.com/u/873584?v=3&amp;s=40" width="20" /> </a>


    </div>

    <div id="blob_contributors_box" style="display:none">
      <h2 class="facebox-header" data-facebox-id="facebox-header">Users who have contributed to this file</h2>
      <ul class="facebox-user-list" data-facebox-id="facebox-description">
          <li class="facebox-user-list-item">
            <img alt="@guodongxiaren" height="24" src="https://avatars2.githubusercontent.com/u/5945107?v=3&amp;s=48" width="24" />
            <a href="/guodongxiaren">guodongxiaren</a>
          </li>
          <li class="facebox-user-list-item">
            <img alt="@jiexishede" height="24" src="https://avatars0.githubusercontent.com/u/9484734?v=3&amp;s=48" width="24" />
            <a href="/jiexishede">jiexishede</a>
          </li>
          <li class="facebox-user-list-item">
            <img alt="@XinHuaLuFang" height="24" src="https://avatars1.githubusercontent.com/u/7486847?v=3&amp;s=48" width="24" />
            <a href="/XinHuaLuFang">XinHuaLuFang</a>
          </li>
          <li class="facebox-user-list-item">
            <img alt="@88250" height="24" src="https://avatars0.githubusercontent.com/u/873584?v=3&amp;s=48" width="24" />
            <a href="/88250">88250</a>
          </li>
      </ul>
    </div>
  </div>

<div class="file">
  <div class="file-header">
  <div class="file-actions">

    <div class="btn-group">
      <a href="/guodongxiaren/README/raw/master/README.md" class="btn btn-sm " id="raw-url">Raw</a>
        <a href="/guodongxiaren/README/blame/master/README.md" class="btn btn-sm js-update-url-with-hash">Blame</a>
      <a href="/guodongxiaren/README/commits/master/README.md" class="btn btn-sm " rel="nofollow">History</a>
    </div>


        <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/guodongxiaren/README/edit/master/README.md" class="inline-form js-update-url-with-hash" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="BlcZdPMB1d4F5ZKM7qtl635ZYTYHaRG2VGmHYKaVv+NtucOhFbjSdUnSWVWv0XBFQsygyVNtvSlw5MpYpWs3vA==" /></div>
          <button class="btn-octicon tooltipped tooltipped-nw" type="submit"
            aria-label="Fork this project and edit the file" data-hotkey="e" data-disable-with>
            <span aria-hidden="true" class="octicon octicon-pencil"></span>
          </button>
</form>        <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="/guodongxiaren/README/delete/master/README.md" class="inline-form" data-form-nonce="5a855038ea7e99088a03f7fecf89f8d9af74a438" method="post"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /><input name="authenticity_token" type="hidden" value="tn6v9b3Zs/qe0MtDkUdq+x2afH8duy7zdOcxoS/CBxTXwL4rFEzVtIKm1eUME0Rm+KJU23J/trZ/V6EtZlrEVA==" /></div>
          <button class="btn-octicon btn-octicon-danger tooltipped tooltipped-nw" type="submit"
            aria-label="Fork this project and delete the file" data-disable-with>
            <span aria-hidden="true" class="octicon octicon-trashcan"></span>
          </button>
</form>  </div>

  <div class="file-info">
      274 lines (220 sloc)
      <span class="file-info-divider"></span>
    8.26 KB
  </div>
</div>

  
  <div id="readme" class="blob instapaper_body">
    <article class="markdown-body entry-content" itemprop="mainContentOfPage"><h1><a id="user-content-readme" class="anchor" href="#readme" aria-hidden="true"><span class="octicon octicon-link"></span></a>README</h1>

<p>该文件用来测试和展示书写README的各种markdown语法。GitHub的markdown语法在标准的markdown语法基础上做了扩充，称之为<code>GitHub Flavored Markdown</code>。简称<code>GFM</code>，GFM在GitHub上有广泛应用，除了README文件外，issues和wiki均支持markdown语法。</p>

<hr>

<h3><a id="user-content-authorjelly" class="anchor" href="#authorjelly" aria-hidden="true"><span class="octicon octicon-link"></span></a>　　　　　　　　　　　　Author:Jelly</h3>

<h3><a id="user-content--e-mail879231132qqcom" class="anchor" href="#-e-mail879231132qqcom" aria-hidden="true"><span class="octicon octicon-link"></span></a>　　　　　　　　　 E-mail:<a href="mailto:879231132@qq.com">879231132@qq.com</a></h3>

<h1></h1>

<h2><a id="user-content-目录" class="anchor" href="#目录" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-index">目录</a></h2><a name="user-content-index">

</a><ul><a name="user-content-index">
</a><li><a name="user-content-index"></a><a href="#line">横线</a></li>
<li><a href="#title">标题</a></li>
<li><a href="#text">文本</a>

<ul>
<li>普通文本</li>
<li>单行文本</li>
<li>多行文本</li>
<li>文字高亮</li>
</ul></li>
<li><a href="#link">链接</a> 

<ul>
<li>文字超链接

<ul>
<li> 链接外部URL</li>
<li> 链接本仓库里的URL</li>
</ul></li>
<li> 锚点</li>
<li><a href="#piclink">图片超链接</a></li>
</ul></li>
<li><a href="#pic">图片</a>

<ul>
<li>来源于网络的图片</li>
<li>GitHub仓库中的图片</li>
</ul></li>
<li><a href="#dot">列表</a>

<ul>
<li>圆点列表</li>
<li>数字列表</li>
<li>复选框列表</li>
</ul></li>
<li><a href="#blockquotes">块引用</a></li>
<li><a href="#code">代码</a></li>
<li><a href="#table">表格</a> </li>
<li><a href="#emoji">表情</a></li>
</ul>

<p><a name="user-content-line"></a></p><a name="user-content-line">

<h2><a id="user-content----___显示虚横线" class="anchor" href="#---___显示虚横线" aria-hidden="true"><span class="octicon octicon-link"></span></a>***、---、___显示虚横线</h2>

<hr>

<hr>

<hr>

</a><p><a name="user-content-line"></a><a name="user-content-title"></a></p><a name="user-content-title">

<h1><a id="user-content-一级标题" class="anchor" href="#一级标题" aria-hidden="true"><span class="octicon octicon-link"></span></a>一级标题</h1>

<h2><a id="user-content-二级标题" class="anchor" href="#二级标题" aria-hidden="true"><span class="octicon octicon-link"></span></a>二级标题</h2>

<h3><a id="user-content-三级标题" class="anchor" href="#三级标题" aria-hidden="true"><span class="octicon octicon-link"></span></a>三级标题</h3>

<h4><a id="user-content-四级标题" class="anchor" href="#四级标题" aria-hidden="true"><span class="octicon octicon-link"></span></a>四级标题</h4>

<h5><a id="user-content-五级标题" class="anchor" href="#五级标题" aria-hidden="true"><span class="octicon octicon-link"></span></a>五级标题</h5>

<h6><a id="user-content-六级标题" class="anchor" href="#六级标题" aria-hidden="true"><span class="octicon octicon-link"></span></a>六级标题</h6>

</a><h2><a id="user-content-显示文本" class="anchor" href="#显示文本" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-title"></a><a name="user-content-text">显示文本</a></h2><a name="user-content-text">

<h3><a id="user-content-普通文本" class="anchor" href="#普通文本" aria-hidden="true"><span class="octicon octicon-link"></span></a>普通文本</h3>

<p>这是一段普通的文本</p>

<h4><a id="user-content-关于换行" class="anchor" href="#关于换行" aria-hidden="true"><span class="octicon octicon-link"></span></a>关于换行</h4>

<p>直接回车不能换行，<br>
可以使用&lt;br&gt;。
但是使用html标签就丧失了markdown的意义。<br>
可以在上一行文本后面补两个空格，<br>
这样下一行的文本就换行了。</p>

<p>或者就是在两行文本直接加一个空行。</p>

<p>也能实现换行效果，不过这个行间距有点大。</p>

<h3><a id="user-content-单行文本" class="anchor" href="#单行文本" aria-hidden="true"><span class="octicon octicon-link"></span></a>单行文本</h3>

<pre><code>Hello,大家好，我是果冻虾仁。
</code></pre>

<h3><a id="user-content-文本块" class="anchor" href="#文本块" aria-hidden="true"><span class="octicon octicon-link"></span></a>文本块</h3>

<pre><code>欢迎到访
很高兴见到您
祝您，早上好，中午好，下午好，晚安
</code></pre>

<h3><a id="user-content-部分文字高亮" class="anchor" href="#部分文字高亮" aria-hidden="true"><span class="octicon octicon-link"></span></a>部分文字高亮</h3>

<p>Thank <code>You</code> . Please <code>Call</code> Me <code>Coder</code></p>

<h4><a id="user-content-高亮功能更适合做一篇文章的tag" class="anchor" href="#高亮功能更适合做一篇文章的tag" aria-hidden="true"><span class="octicon octicon-link"></span></a>高亮功能更适合做一篇文章的tag</h4>

<p>例如:<br>
<code>java</code> <code>网络编程</code> <code>Socket</code> <code>全双工</code></p>

<h4><a id="user-content-删除线" class="anchor" href="#删除线" aria-hidden="true"><span class="octicon octicon-link"></span></a>删除线</h4>

<p>这是一个 <del>删除线</del></p>

<h4><a id="user-content-斜体" class="anchor" href="#斜体" aria-hidden="true"><span class="octicon octicon-link"></span></a>斜体</h4>

<p><em>斜体1</em></p>

<p><em>斜体2</em></p>

<h4><a id="user-content-粗体" class="anchor" href="#粗体" aria-hidden="true"><span class="octicon octicon-link"></span></a>粗体</h4>

<p><strong>粗体1</strong></p>

<p><strong>粗体2</strong></p>

<h4><a id="user-content-组合使用粗体斜体和删除线" class="anchor" href="#组合使用粗体斜体和删除线" aria-hidden="true"><span class="octicon octicon-link"></span></a>组合使用粗体、斜体和删除线</h4>

<p><strong><em>斜粗体1</em></strong></p>

<p><strong><em>斜粗体2</em></strong></p>

<p><strong><em><del>斜粗体删除线1</del></em></strong></p>

<p><del><strong><em>斜粗体删除线2</em></strong></del></p>

</a><h2><a id="user-content-链接" class="anchor" href="#链接" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-text"></a><a name="user-content-link">链接</a></h2><a name="user-content-link">

<h3><a id="user-content-链接外部url" class="anchor" href="#链接外部url" aria-hidden="true"><span class="octicon octicon-link"></span></a>链接外部URL</h3>

</a><p><a name="user-content-link"></a><a href="http://blog.csdn.net/guodongxiaren" title="悬停显示">我的博客</a>   语法如下：</p>

<pre><code>[我的博客](http://blog.csdn.net/guodongxiaren "悬停显示")
</code></pre>

<h3><a id="user-content-链接的另一种写法" class="anchor" href="#链接的另一种写法" aria-hidden="true"><span class="octicon octicon-link"></span></a>链接的另一种写法</h3>

<p><a href="http://blog.csdn.net/guodongxiaren" title="悬停显示">我的博客</a>  </p>

<p>语法如下：</p>

<pre><code>[我的博客][id]
[id]:http://blog.csdn.net/guodongxiaren "悬停显示"
</code></pre>

<p>中括号[ ]里的id，可以是数字，字母等的组合。这两行可以不连着写，<strong>一般把第二行的链接统一放在文章末尾</strong>，id上下对应就行了。这样正文看起来会比较干净。</p>

<h3><a id="user-content-链接本仓库里的url" class="anchor" href="#链接本仓库里的url" aria-hidden="true"><span class="octicon octicon-link"></span></a>链接本仓库里的URL</h3>

<p><a href="/guodongxiaren/README/blob/master/Book">Book</a>
语法如下：</p>

<pre><code>[Book](./Book)
</code></pre>

<p>如果文件要引用的文件不存在，则待点击的文本为红色。引用的文件存在存在则文本为蓝色。</p>

<h3><a id="user-content-锚点" class="anchor" href="#锚点" aria-hidden="true"><span class="octicon octicon-link"></span></a>锚点</h3>

<p>我们可以使用HTML的锚点标签（<code>#</code>）来设置锚点：<a href="#index">回到目录</a><br>
但其实呢，每一个标题都是一个锚点，不需要用标签来指定，比如我们 <a href="#TEST">回到顶部</a>
不过不幸的是，由于对中文支持的不好，所以中文标题貌似是不能视作标签的。</p>

<h2><a id="user-content-显示图片" class="anchor" href="#显示图片" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-pic">显示图片</a></h2><a name="user-content-pic">

<h3><a id="user-content-来源于网络的图片" class="anchor" href="#来源于网络的图片" aria-hidden="true"><span class="octicon octicon-link"></span></a>来源于网络的图片</h3>

<p><img src="https://camo.githubusercontent.com/15675678891dead0d516b6ee7a57ed12101ce69a/687474703a2f2f7777772e62616964752e636f6d2f696d672f62646c6f676f2e676966" alt="baidu" title="百度logo" data-canonical-src="http://www.baidu.com/img/bdlogo.gif" style="max-width:100%;">
<img src="https://camo.githubusercontent.com/2420cd05cf9ec005edbe5978741777f7a7ac746b/68747470733a2f2f6173736574732d63646e2e6769746875622e636f6d2f696d616765732f6d6f64756c65732f636f6e746163742f676f6c64737461722e676966" alt="" data-canonical-src="https://assets-cdn.github.com/images/modules/contact/goldstar.gif" style="max-width:100%;"></p>

<h3><a id="user-content-github仓库中的图片" class="anchor" href="#github仓库中的图片" aria-hidden="true"><span class="octicon octicon-link"></span></a>GitHub仓库中的图片</h3>

<p><img src="https://github.com/guodongxiaren/ImageCache/raw/master/Logo/foryou.gif" alt="" style="max-width:100%;"></p>

</a><h3><a id="user-content-给图片加上超链接" class="anchor" href="#给图片加上超链接" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-pic"></a><a name="user-content-piclink">给图片加上超链接</a></h3><a name="user-content-piclink">

<h4><a id="user-content-第一种" class="anchor" href="#第一种" aria-hidden="true"><span class="octicon octicon-link"></span></a>第一种</h4>

</a><p><a name="user-content-piclink"></a><a href="http://blog.csdn.net/guodongxiaren/article/details/23690801"><img src="https://github.com/guodongxiaren/ImageCache/raw/master/Logo/jianxin.jpg" alt="head" title="点击图片进入我的博客" style="max-width:100%;"></a></p>

<h4><a id="user-content-第二种" class="anchor" href="#第二种" aria-hidden="true"><span class="octicon octicon-link"></span></a>第二种</h4>

<p><a href="http://www.baidu.com"><img src="https://camo.githubusercontent.com/15675678891dead0d516b6ee7a57ed12101ce69a/687474703a2f2f7777772e62616964752e636f6d2f696d672f62646c6f676f2e676966" alt="内容任意" title="百度logo" data-canonical-src="http://www.baidu.com/img/bdlogo.gif" style="max-width:100%;"></a></p>

<h2><a id="user-content-列表" class="anchor" href="#列表" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-dot">列表</a></h2><a name="user-content-dot">

<h3><a id="user-content-圆点列表" class="anchor" href="#圆点列表" aria-hidden="true"><span class="octicon octicon-link"></span></a>圆点列表</h3>

<ul>
<li>昵称：果冻虾仁</li>
<li>别名：隔壁老王</li>
<li>英文名：Jelly</li>
</ul>

<h3><a id="user-content-更多圆点" class="anchor" href="#更多圆点" aria-hidden="true"><span class="octicon octicon-link"></span></a>更多圆点</h3>

<ul>
<li>编程语言

<ul>
<li>脚本语言

<ul>
<li>Python</li>
</ul></li>
</ul></li>
</ul>

<h3><a id="user-content-数字列表" class="anchor" href="#数字列表" aria-hidden="true"><span class="octicon octicon-link"></span></a>数字列表</h3>

<h4><a id="user-content-一般效果" class="anchor" href="#一般效果" aria-hidden="true"><span class="octicon octicon-link"></span></a>一般效果</h4>

<p>就是在数字后面加一个点，再加一个空格。不过看起来起来可能不够明显。<br>
面向对象的三个基本特征：</p>

<ol>
<li>封装</li>
<li>继承</li>
<li>多态</li>
</ol>

<h4><a id="user-content-数字列表自动排序" class="anchor" href="#数字列表自动排序" aria-hidden="true"><span class="octicon octicon-link"></span></a>数字列表自动排序</h4>

<p>也可以在第一行指定<code>1.</code>，而接下来的几行用星号<code>*</code>（或者继续用数字1. ）就可以了，它会自动显示成2、3、4……。<br>
面向对象的七大原则：</p>

<ol>
<li>开闭原则</li>
<li>里氏转换原则</li>
<li>依赖倒转原则</li>
<li>接口隔离原则</li>
<li>组合/聚合复用原则</li>
<li>“迪米特”法则</li>
<li>单一直则原则</li>
</ol>

<h4><a id="user-content-多级数字列表" class="anchor" href="#多级数字列表" aria-hidden="true"><span class="octicon octicon-link"></span></a>多级数字列表</h4>

<p>和圆点的列表一样，数字列表也有多级结构：  </p>

<ol>
<li>这是一级的数字列表，数字1还是1

<ol>
<li>这是二级的数字列表，阿拉伯数字在显示的时候变成了罗马数字

<ol>
<li>这是三级的数字列表，数字在显示的时候变成了英文字母

<ol>
<li>四级的数字列表显示效果，就不再变化了，依旧是英文字母</li>
</ol></li>
</ol></li>
</ol></li>
</ol>

<h3><a id="user-content-复选框列表" class="anchor" href="#复选框列表" aria-hidden="true"><span class="octicon octicon-link"></span></a>复选框列表</h3>

<ul class="task-list">
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" checked="checked" disabled="disabled"> C</li>
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" checked="checked" disabled="disabled"> C++</li>
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" checked="checked" disabled="disabled"> Java</li>
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" checked="checked" disabled="disabled"> Qt</li>
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" checked="checked" disabled="disabled"> Android</li>
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" disabled="disabled"> C#</li>
<li class="task-list-item"><input type="checkbox" class="task-list-item-checkbox" disabled="disabled"> .NET</li>
</ul>

<p>您可以使用这个功能来标注某个项目各项任务的完成情况。</p>

</a><h2><a id="user-content-块引用" class="anchor" href="#块引用" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-dot"></a><a name="user-content-blockquotes">块引用</a></h2><a name="user-content-blockquotes">

<h3><a id="user-content-常用于引用文本" class="anchor" href="#常用于引用文本" aria-hidden="true"><span class="octicon octicon-link"></span></a>常用于引用文本</h3>

<h4><a id="user-content-文本摘自深入理解计算机系统p27" class="anchor" href="#文本摘自深入理解计算机系统p27" aria-hidden="true"><span class="octicon octicon-link"></span></a>文本摘自《深入理解计算机系统》P27</h4>

<p>　令人吃惊的是，在哪种字节顺序是合适的这个问题上，人们表现得非常情绪化。实际上术语“little endian”（小端）和“big endian”（大端）出自Jonathan Swift的《格利佛游记》一书，其中交战的两个派别无法就应该从哪一端打开一个半熟的鸡蛋达成一致。因此，争论沦为关于社会政治的争论。只要选择了一种规则并且始终如一的坚持，其实对于哪种字节排序的选择都是任意的。</p>

<blockquote>
<p><b>“端”（endian）的起源</b><br>
以下是Jonathan Swift在1726年关于大小端之争历史的描述：<br>
“……下面我要告诉你的是，Lilliput和Blefuscu这两大强国在过去36个月里一直在苦战。战争开始是由于以下的原因：我们大家都认为，吃鸡蛋前，原始的方法是打破鸡蛋较大的一端，可是当今的皇帝的祖父小时候吃鸡蛋，一次按古法打鸡蛋时碰巧将一个手指弄破了，因此他的父亲，当时的皇帝，就下了一道敕令，命令全体臣民吃鸡蛋时打破较小的一端，违令者重罚。”</p>
</blockquote>

<h3><a id="user-content-块引用有多级结构" class="anchor" href="#块引用有多级结构" aria-hidden="true"><span class="octicon octicon-link"></span></a>块引用有多级结构</h3>

<blockquote>
<p>数据结构</p>

<blockquote>
<p>树</p>

<blockquote>
<p>二叉树</p>

<blockquote>
<p>平衡二叉树</p>

<blockquote>
<p>满二叉树</p>
</blockquote>
</blockquote>
</blockquote>
</blockquote>
</blockquote>

</a><h2><a id="user-content-代码高亮" class="anchor" href="#代码高亮" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-blockquotes"></a><a name="user-content-code">代码高亮</a></h2><a name="user-content-code">

<div class="highlight highlight-source-java"><pre><span class="pl-k">public</span> <span class="pl-k">static</span> <span class="pl-k">void</span> main(<span class="pl-k">String</span>[]args){} <span class="pl-c">//Java</span></pre></div>

<div class="highlight highlight-source-c"><pre><span class="pl-k">int</span> <span class="pl-en">main</span>(<span class="pl-k">int</span> argc, <span class="pl-k">char</span> *argv[]) <span class="pl-c">//C</span></pre></div>

<div class="highlight highlight-source-shell"><pre><span class="pl-c1">echo</span> <span class="pl-s"><span class="pl-pds">"</span>hello GitHub<span class="pl-pds">"</span></span>#Bash</pre></div>

<div class="highlight highlight-source-js"><pre><span class="pl-smi">document</span>.<span class="pl-c1">getElementById</span>(<span class="pl-s"><span class="pl-pds">"</span>myH1<span class="pl-pds">"</span></span>).<span class="pl-smi">innerHTML</span><span class="pl-k">=</span><span class="pl-s"><span class="pl-pds">"</span>Welcome to my Homepage<span class="pl-pds">"</span></span>; <span class="pl-c">//javascipt</span></pre></div>

<div class="highlight highlight-source-c++"><pre>string &amp;<span class="pl-k">operator</span>+(<span class="pl-k">const</span> string&amp; A,<span class="pl-k">const</span> string&amp; B) <span class="pl-c">//cpp</span></pre></div>

</a><h2><a id="user-content-显示表格" class="anchor" href="#显示表格" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-code"></a><a name="user-content-table">显示表格</a></h2><a name="user-content-table">

<table><thead>
<tr>
<th>表头1</th>
<th>表头2</th>
</tr>
</thead><tbody>
<tr>
<td>Content Cell</td>
<td>Content Cell</td>
</tr>
<tr>
<td>Content Cell</td>
<td>Content Cell</td>
</tr>
</tbody></table>

<table><thead>
<tr>
<th>表头1</th>
<th>表头2</th>
</tr>
</thead><tbody>
<tr>
<td>Content Cell</td>
<td>Content Cell</td>
</tr>
<tr>
<td>Content Cell</td>
<td>Content Cell</td>
</tr>
</tbody></table>

<table><thead>
<tr>
<th>名字</th>
<th>描述</th>
</tr>
</thead><tbody>
<tr>
<td>Help</td>
<td>Display the help window.</td>
</tr>
<tr>
<td>Close</td>
<td>Closes a window</td>
</tr>
</tbody></table>

<p>表格中也可以使用普通文本的删除线，斜体等效果</p>

<table><thead>
<tr>
<th>名字</th>
<th>描述</th>
</tr>
</thead><tbody>
<tr>
<td>Help</td>
<td><del>Display the</del> help window.</td>
</tr>
<tr>
<td>Close</td>
<td><em>Closes</em> a window</td>
</tr>
</tbody></table>

<p>表格可以指定对齐方式</p>

<table><thead>
<tr>
<th align="left">左对齐</th>
<th align="center">居中</th>
<th align="right">右对齐</th>
</tr>
</thead><tbody>
<tr>
<td align="left">col 3 is</td>
<td align="center">some wordy text</td>
<td align="right">$1600</td>
</tr>
<tr>
<td align="left">col 2 is</td>
<td align="center">centered</td>
<td align="right">$12</td>
</tr>
<tr>
<td align="left">zebra stripes</td>
<td align="center">are neat</td>
<td align="right">$1</td>
</tr>
</tbody></table>

<p>表格中嵌入图片</p>

<table><thead>
<tr>
<th>图片</th>
<th>描述</th>
</tr>
</thead><tbody>
<tr>
<td><img src="https://camo.githubusercontent.com/15675678891dead0d516b6ee7a57ed12101ce69a/687474703a2f2f7777772e62616964752e636f6d2f696d672f62646c6f676f2e676966" alt="baidu" title="百度logo" data-canonical-src="http://www.baidu.com/img/bdlogo.gif" style="max-width:100%;"></td>
<td>baidu</td>
</tr>
</tbody></table>

</a><h2><a id="user-content-添加表情" class="anchor" href="#添加表情" aria-hidden="true"><span class="octicon octicon-link"></span></a><a name="user-content-table"></a><a name="user-content-emoji">添加表情</a></h2><a name="user-content-emoji">

<p>Github的Markdown语法支持添加emoji表情，输入不同的符号码（两个冒号包围的字符）可以显示出不同的表情。</p>

<p>比如<code>:blush:</code>，可以显示<img class="emoji" title=":blush:" alt=":blush:" src="https://assets-cdn.github.com/images/icons/emoji/unicode/1f60a.png" height="20" width="20" align="absmiddle">。</p>

</a><p><a name="user-content-emoji">具体每一个表情的符号码，可以查询GitHub的官方网页</a><a href="http://www.emoji-cheat-sheet.com">http://www.emoji-cheat-sheet.com</a>。</p>

<p>但是这个网页每次都打开<strong>奇慢</strong>。。所以我整理到了本repo中，大家可以直接在此查看<a href="/guodongxiaren/README/blob/master/emoji.md">emoji</a>。</p>
</article>
  </div>

</div>

<a href="#jump-to-line" rel="facebox[.linejump]" data-hotkey="l" style="display:none">Jump to Line</a>
<div id="jump-to-line" style="display:none">
  <!-- </textarea> --><!-- '"` --><form accept-charset="UTF-8" action="" class="js-jump-to-line-form" method="get"><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&#x2713;" /></div>
    <input class="linejump-input js-jump-to-line-field" type="text" placeholder="Jump to line&hellip;" aria-label="Jump to line" autofocus>
    <button type="submit" class="btn">Go</button>
</form></div>

  </div>
  <div class="modal-backdrop"></div>
</div>

    </div>
  </div>

    </div>

        <div class="container">
  <div class="site-footer" role="contentinfo">
    <ul class="site-footer-links right">
        <li><a href="https://status.github.com/" data-ga-click="Footer, go to status, text:status">Status</a></li>
      <li><a href="https://developer.github.com" data-ga-click="Footer, go to api, text:api">API</a></li>
      <li><a href="https://training.github.com" data-ga-click="Footer, go to training, text:training">Training</a></li>
      <li><a href="https://shop.github.com" data-ga-click="Footer, go to shop, text:shop">Shop</a></li>
        <li><a href="https://github.com/blog" data-ga-click="Footer, go to blog, text:blog">Blog</a></li>
        <li><a href="https://github.com/about" data-ga-click="Footer, go to about, text:about">About</a></li>
        <li><a href="https://github.com/pricing" data-ga-click="Footer, go to pricing, text:pricing">Pricing</a></li>

    </ul>

    <a href="https://github.com" aria-label="Homepage">
      <span aria-hidden="true" class="mega-octicon octicon-mark-github" title="GitHub "></span>
</a>
    <ul class="site-footer-links">
      <li>&copy; 2016 <span title="0.07478s from github-fe145-cp1-prd.iad.github.net">GitHub</span>, Inc.</li>
        <li><a href="https://github.com/site/terms" data-ga-click="Footer, go to terms, text:terms">Terms</a></li>
        <li><a href="https://github.com/site/privacy" data-ga-click="Footer, go to privacy, text:privacy">Privacy</a></li>
        <li><a href="https://github.com/security" data-ga-click="Footer, go to security, text:security">Security</a></li>
        <li><a href="https://github.com/contact" data-ga-click="Footer, go to contact, text:contact">Contact</a></li>
        <li><a href="https://help.github.com" data-ga-click="Footer, go to help, text:help">Help</a></li>
    </ul>
  </div>
</div>



    
    
    

    <div id="ajax-error-message" class="flash flash-error">
      <span aria-hidden="true" class="octicon octicon-alert"></span>
      <button type="button" class="flash-close js-flash-close js-ajax-error-dismiss" aria-label="Dismiss error">
        <span aria-hidden="true" class="octicon octicon-x"></span>
      </button>
      Something went wrong with that request. Please try again.
    </div>


      
      <script crossorigin="anonymous" integrity="sha256-nuVc6vh/w03IYzQkn+9svs6I6BVHjg++gWQtV+0P/4k=" src="https://assets-cdn.github.com/assets/frameworks-9ee55ceaf87fc34dc86334249fef6cbece88e815478e0fbe81642d57ed0fff89.js"></script>
      <script async="async" crossorigin="anonymous" integrity="sha256-myNyouhsB4E/qZiMz3Oasnq97ikJZ1iJkbsZ3V28iC0=" src="https://assets-cdn.github.com/assets/github-9b2372a2e86c07813fa9988ccf739ab27abdee290967588991bb19dd5dbc882d.js"></script>
      
      
      
    <div class="js-stale-session-flash stale-session-flash flash flash-warn flash-banner hidden">
      <span aria-hidden="true" class="octicon octicon-alert"></span>
      <span class="signed-in-tab-flash">You signed in with another tab or window. <a href="">Reload</a> to refresh your session.</span>
      <span class="signed-out-tab-flash">You signed out in another tab or window. <a href="">Reload</a> to refresh your session.</span>
    </div>
    <div class="facebox" id="facebox" style="display:none;">
  <div class="facebox-popup">
    <div class="facebox-content" role="dialog" aria-labelledby="facebox-header" aria-describedby="facebox-description">
    </div>
    <button type="button" class="facebox-close js-facebox-close" aria-label="Close modal">
      <span aria-hidden="true" class="octicon octicon-x"></span>
    </button>
  </div>
</div>

  </body>
</html>

