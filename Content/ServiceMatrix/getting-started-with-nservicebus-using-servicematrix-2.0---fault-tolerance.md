




<!DOCTYPE html>
<html>
  <head prefix="og: http://ogp.me/ns# fb: http://ogp.me/ns/fb# object: http://ogp.me/ns/object# article: http://ogp.me/ns/article# profile: http://ogp.me/ns/profile#">
    <meta charset='utf-8'>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <title>docs.particular.net/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md at Drafts · Particular/docs.particular.net</title>
    <link rel="search" type="application/opensearchdescription+xml" href="/opensearch.xml" title="GitHub" />
    <link rel="fluid-icon" href="https://github.com/fluidicon.png" title="GitHub" />
    <link rel="apple-touch-icon" sizes="57x57" href="/apple-touch-icon-114.png" />
    <link rel="apple-touch-icon" sizes="114x114" href="/apple-touch-icon-114.png" />
    <link rel="apple-touch-icon" sizes="72x72" href="/apple-touch-icon-144.png" />
    <link rel="apple-touch-icon" sizes="144x144" href="/apple-touch-icon-144.png" />
    <meta property="fb:app_id" content="1401488693436528"/>

      <meta content="@github" name="twitter:site" /><meta content="summary" name="twitter:card" /><meta content="Particular/docs.particular.net" name="twitter:title" /><meta content="docs.particular.net - Documentation" name="twitter:description" /><meta content="https://1.gravatar.com/avatar/64c9a3fc7d60e0fa8f4c0782a807f54f?d=https%3A%2F%2Fidenticons.github.com%2Fba789ddab74eaf56456dc122aab5e087.png&amp;r=x&amp;s=400" name="twitter:image:src" />
<meta content="GitHub" property="og:site_name" /><meta content="object" property="og:type" /><meta content="https://1.gravatar.com/avatar/64c9a3fc7d60e0fa8f4c0782a807f54f?d=https%3A%2F%2Fidenticons.github.com%2Fba789ddab74eaf56456dc122aab5e087.png&amp;r=x&amp;s=400" property="og:image" /><meta content="Particular/docs.particular.net" property="og:title" /><meta content="https://github.com/Particular/docs.particular.net" property="og:url" /><meta content="docs.particular.net - Documentation" property="og:description" />

    <meta name="hostname" content="github-fe132-cp1-prd.iad.github.net">
    <meta name="ruby" content="ruby 2.1.0p0-github-tcmalloc (87d8860372) [x86_64-linux]">
    <link rel="assets" href="https://github.global.ssl.fastly.net/">
    <link rel="conduit-xhr" href="https://ghconduit.com:25035/">
    <link rel="xhr-socket" href="/_sockets" />
    


    <meta name="msapplication-TileImage" content="/windows-tile.png" />
    <meta name="msapplication-TileColor" content="#ffffff" />
    <meta name="selected-link" value="repo_source" data-pjax-transient />
    <meta content="collector.githubapp.com" name="octolytics-host" /><meta content="collector-cdn.github.com" name="octolytics-script-host" /><meta content="github" name="octolytics-app-id" /><meta content="D06B169D:5593:2336012:52EB3ABC" name="octolytics-dimension-request_id" /><meta content="3620645" name="octolytics-actor-id" /><meta content="jdrat2000" name="octolytics-actor-login" /><meta content="3fe7e016aae66c78339e4525d9cd98a7bfad4acc9cd0eec3de89284dadf9054d" name="octolytics-actor-hash" />
    

    
    
    <link rel="icon" type="image/x-icon" href="/favicon.ico" />

    <meta content="authenticity_token" name="csrf-param" />
<meta content="nSnsVcwkFZlDqM/hN6JQ+AXRziKR4KREnCV1OH2OoCE=" name="csrf-token" />

    <link href="https://github.global.ssl.fastly.net/assets/github-ffe7e45502ed615bf3cb9cb1bbb7d0d32f095264.css" media="all" rel="stylesheet" type="text/css" />
    <link href="https://github.global.ssl.fastly.net/assets/github2-13805a78bc89e6b8baa64d96ee8bef168f18cedb.css" media="all" rel="stylesheet" type="text/css" />
    


      <script src="https://github.global.ssl.fastly.net/assets/frameworks-3e59bf2ccf0be318d5d920c2ab0bf1ab4cb3a96b.js" type="text/javascript"></script>
      <script async="async" defer="defer" src="https://github.global.ssl.fastly.net/assets/github-83b72d1b7fb478c00875144cd0745940018b2aa0.js" type="text/javascript"></script>
      
      <meta http-equiv="x-pjax-version" content="70a4615a2cb06e5e2339bf9a43551603">

        <link data-pjax-transient rel='permalink' href='/Particular/docs.particular.net/blob/e76ca48dbec0abb452a0ab3b0c3906348f170c0f/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md'>

  <meta name="description" content="docs.particular.net - Documentation" />

  <meta content="5038441" name="octolytics-dimension-user_id" /><meta content="Particular" name="octolytics-dimension-user_login" /><meta content="11727301" name="octolytics-dimension-repository_id" /><meta content="Particular/docs.particular.net" name="octolytics-dimension-repository_nwo" /><meta content="true" name="octolytics-dimension-repository_public" /><meta content="false" name="octolytics-dimension-repository_is_fork" /><meta content="11727301" name="octolytics-dimension-repository_network_root_id" /><meta content="Particular/docs.particular.net" name="octolytics-dimension-repository_network_root_nwo" />
  <link href="https://github.com/Particular/docs.particular.net/commits/Drafts.atom" rel="alternate" title="Recent Commits to docs.particular.net:Drafts" type="application/atom+xml" />

  </head>


  <body class="logged_in  env-production windows vis-public page-blob">
    <div class="wrapper">
      
      
      
      


      <div class="header header-logged-in true">
  <div class="container clearfix">

    <a class="header-logo-invertocat" href="https://github.com/">
  <span class="mega-octicon octicon-mark-github"></span>
</a>

    
    <a href="/Particular/docs.particular.net/notifications" class="notification-indicator tooltipped downwards contextually-unread" data-gotokey="n" title="You have unread notifications in this repository">
        <span class="mail-status unread"></span>
</a>

      <div class="command-bar js-command-bar  in-repository">
          <form accept-charset="UTF-8" action="/search" class="command-bar-form" id="top_search_form" method="get">

<input type="text" data-hotkey="/ s" name="q" id="js-command-bar-field" placeholder="Search or type a command" tabindex="1" autocapitalize="off"
    
    data-username="jdrat2000"
      data-repo="Particular/docs.particular.net"
      data-branch="Drafts"
      data-sha="e873d58cfbee044a351eb79697a71c5d14730be2"
  >

    <input type="hidden" name="nwo" value="Particular/docs.particular.net" />

    <div class="select-menu js-menu-container js-select-menu search-context-select-menu">
      <span class="minibutton select-menu-button js-menu-target">
        <span class="js-select-button">This repository</span>
      </span>

      <div class="select-menu-modal-holder js-menu-content js-navigation-container">
        <div class="select-menu-modal">

          <div class="select-menu-item js-navigation-item js-this-repository-navigation-item selected">
            <span class="select-menu-item-icon octicon octicon-check"></span>
            <input type="radio" class="js-search-this-repository" name="search_target" value="repository" checked="checked" />
            <div class="select-menu-item-text js-select-button-text">This repository</div>
          </div> <!-- /.select-menu-item -->

          <div class="select-menu-item js-navigation-item js-all-repositories-navigation-item">
            <span class="select-menu-item-icon octicon octicon-check"></span>
            <input type="radio" name="search_target" value="global" />
            <div class="select-menu-item-text js-select-button-text">All repositories</div>
          </div> <!-- /.select-menu-item -->

        </div>
      </div>
    </div>

  <span class="octicon help tooltipped downwards" title="Show command bar help">
    <span class="octicon octicon-question"></span>
  </span>


  <input type="hidden" name="ref" value="cmdform">

</form>
        <ul class="top-nav">
          <li class="explore"><a href="/explore">Explore</a></li>
            <li><a href="https://gist.github.com">Gist</a></li>
            <li><a href="/blog">Blog</a></li>
          <li><a href="https://help.github.com">Help</a></li>
        </ul>
      </div>

    


  <ul id="user-links">
    <li>
      <a href="/jdrat2000" class="name">
        <img alt="Joe R" height="20" src="https://0.gravatar.com/avatar/80dce63dc7d30e4cc0b23cf5999a5875?d=https%3A%2F%2Fidenticons.github.com%2F64922cc573d0357c7ce7e5789dd67998.png&amp;r=x&amp;s=140" width="20" /> jdrat2000
      </a>
    </li>

    <li class="new-menu dropdown-toggle js-menu-container">
      <a href="#" class="js-menu-target tooltipped downwards" title="Create new..." aria-label="Create new...">
        <span class="octicon octicon-plus"></span>
        <span class="dropdown-arrow"></span>
      </a>

      <div class="js-menu-content">
      </div>
    </li>

    <li>
      <a href="/settings/profile" id="account_settings"
        class="tooltipped downwards"
        aria-label="Account settings "
        title="Account settings ">
        <span class="octicon octicon-tools"></span>
      </a>
    </li>
    <li>
      <a class="tooltipped downwards" href="/logout" data-method="post" id="logout" title="Sign out" aria-label="Sign out">
        <span class="octicon octicon-log-out"></span>
      </a>
    </li>

  </ul>

<div class="js-new-dropdown-contents hidden">
  

<ul class="dropdown-menu">
  <li>
    <a href="/new"><span class="octicon octicon-repo-create"></span> New repository</a>
  </li>
  <li>
    <a href="/organizations/new"><span class="octicon octicon-organization"></span> New organization</a>
  </li>


    <li class="section-title">
      <span title="Particular/docs.particular.net">This repository</span>
    </li>
      <li>
        <a href="/Particular/docs.particular.net/issues/new"><span class="octicon octicon-issue-opened"></span> New issue</a>
      </li>
</ul>

</div>


    
  </div>
</div>

      

      




          <div class="site" itemscope itemtype="http://schema.org/WebPage">
    
    <div class="pagehead repohead instapaper_ignore readability-menu">
      <div class="container">
        

<ul class="pagehead-actions">

    <li class="subscription">
      <form accept-charset="UTF-8" action="/notifications/subscribe" class="js-social-container" data-autosubmit="true" data-remote="true" method="post"><div style="margin:0;padding:0;display:inline"><input name="authenticity_token" type="hidden" value="nSnsVcwkFZlDqM/hN6JQ+AXRziKR4KREnCV1OH2OoCE=" /></div>  <input id="repository_id" name="repository_id" type="hidden" value="11727301" />

    <div class="select-menu js-menu-container js-select-menu">
      <a class="social-count js-social-count" href="/Particular/docs.particular.net/watchers">
        19
      </a>
      <span class="minibutton select-menu-button with-count js-menu-target" role="button" tabindex="0">
        <span class="js-select-button">
          <span class="octicon octicon-eye-unwatch"></span>
          Unwatch
        </span>
      </span>

      <div class="select-menu-modal-holder">
        <div class="select-menu-modal subscription-menu-modal js-menu-content">
          <div class="select-menu-header">
            <span class="select-menu-title">Notification status</span>
            <span class="octicon octicon-remove-close js-menu-close"></span>
          </div> <!-- /.select-menu-header -->

          <div class="select-menu-list js-navigation-container" role="menu">

            <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
              <span class="select-menu-item-icon octicon octicon-check"></span>
              <div class="select-menu-item-text">
                <input id="do_included" name="do" type="radio" value="included" />
                <h4>Not watching</h4>
                <span class="description">You only receive notifications for conversations in which you participate or are @mentioned.</span>
                <span class="js-select-button-text hidden-select-button-text">
                  <span class="octicon octicon-eye-watch"></span>
                  Watch
                </span>
              </div>
            </div> <!-- /.select-menu-item -->

            <div class="select-menu-item js-navigation-item selected" role="menuitem" tabindex="0">
              <span class="select-menu-item-icon octicon octicon octicon-check"></span>
              <div class="select-menu-item-text">
                <input checked="checked" id="do_subscribed" name="do" type="radio" value="subscribed" />
                <h4>Watching</h4>
                <span class="description">You receive notifications for all conversations in this repository.</span>
                <span class="js-select-button-text hidden-select-button-text">
                  <span class="octicon octicon-eye-unwatch"></span>
                  Unwatch
                </span>
              </div>
            </div> <!-- /.select-menu-item -->

            <div class="select-menu-item js-navigation-item " role="menuitem" tabindex="0">
              <span class="select-menu-item-icon octicon octicon-check"></span>
              <div class="select-menu-item-text">
                <input id="do_ignore" name="do" type="radio" value="ignore" />
                <h4>Ignoring</h4>
                <span class="description">You do not receive any notifications for conversations in this repository.</span>
                <span class="js-select-button-text hidden-select-button-text">
                  <span class="octicon octicon-mute"></span>
                  Stop ignoring
                </span>
              </div>
            </div> <!-- /.select-menu-item -->

          </div> <!-- /.select-menu-list -->

        </div> <!-- /.select-menu-modal -->
      </div> <!-- /.select-menu-modal-holder -->
    </div> <!-- /.select-menu -->

</form>
    </li>

  <li>
  

  <div class="js-toggler-container js-social-container starring-container ">
    <a href="/Particular/docs.particular.net/unstar"
      class="minibutton with-count js-toggler-target star-button starred upwards"
      title="Unstar this repository" data-remote="true" data-method="post" rel="nofollow">
      <span class="octicon octicon-star-delete"></span><span class="text">Unstar</span>
    </a>

    <a href="/Particular/docs.particular.net/star"
      class="minibutton with-count js-toggler-target star-button unstarred upwards"
      title="Star this repository" data-remote="true" data-method="post" rel="nofollow">
      <span class="octicon octicon-star"></span><span class="text">Star</span>
    </a>

      <a class="social-count js-social-count" href="/Particular/docs.particular.net/stargazers">
        1
      </a>
  </div>

  </li>


        <li>
          <a href="/Particular/docs.particular.net/fork" class="minibutton with-count js-toggler-target fork-button lighter upwards" title="Fork this repo" rel="facebox nofollow">
            <span class="octicon octicon-git-branch-create"></span><span class="text">Fork</span>
          </a>
          <a href="/Particular/docs.particular.net/network" class="social-count">5</a>
        </li>


</ul>

        <h1 itemscope itemtype="http://data-vocabulary.org/Breadcrumb" class="entry-title public">
          <span class="repo-label"><span>public</span></span>
          <span class="mega-octicon octicon-repo"></span>
          <span class="author">
            <a href="/Particular" class="url fn" itemprop="url" rel="author"><span itemprop="title">Particular</span></a>
          </span>
          <span class="repohead-name-divider">/</span>
          <strong><a href="/Particular/docs.particular.net" class="js-current-repository js-repo-home-link">docs.particular.net</a></strong>

          <span class="page-context-loader">
            <img alt="Octocat-spinner-32" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
          </span>

        </h1>
      </div><!-- /.container -->
    </div><!-- /.repohead -->

    <div class="container">
      

      <div class="repository-with-sidebar repo-container new-discussion-timeline js-new-discussion-timeline  ">
        <div class="repository-sidebar">
            

<div class="sunken-menu vertical-right repo-nav js-repo-nav js-repository-container-pjax js-octicon-loaders">
  <div class="sunken-menu-contents">
    <ul class="sunken-menu-group">
      <li class="tooltipped leftwards" title="Code">
        <a href="/Particular/docs.particular.net/tree/Drafts" aria-label="Code" class="selected js-selected-navigation-item sunken-menu-item" data-gotokey="c" data-pjax="true" data-selected-links="repo_source repo_downloads repo_commits repo_tags repo_branches /Particular/docs.particular.net/tree/Drafts">
          <span class="octicon octicon-code"></span> <span class="full-word">Code</span>
          <img alt="Octocat-spinner-32" class="mini-loader" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
</a>      </li>

        <li class="tooltipped leftwards" title="Issues">
          <a href="/Particular/docs.particular.net/issues" aria-label="Issues" class="js-selected-navigation-item sunken-menu-item js-disable-pjax" data-gotokey="i" data-selected-links="repo_issues /Particular/docs.particular.net/issues">
            <span class="octicon octicon-issue-opened"></span> <span class="full-word">Issues</span>
            <span class='counter'>106</span>
            <img alt="Octocat-spinner-32" class="mini-loader" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
</a>        </li>

      <li class="tooltipped leftwards" title="Pull Requests">
        <a href="/Particular/docs.particular.net/pulls" aria-label="Pull Requests" class="js-selected-navigation-item sunken-menu-item js-disable-pjax" data-gotokey="p" data-selected-links="repo_pulls /Particular/docs.particular.net/pulls">
            <span class="octicon octicon-git-pull-request"></span> <span class="full-word">Pull Requests</span>
            <span class='counter'>1</span>
            <img alt="Octocat-spinner-32" class="mini-loader" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
</a>      </li>


    </ul>
    <div class="sunken-menu-separator"></div>
    <ul class="sunken-menu-group">

      <li class="tooltipped leftwards" title="Pulse">
        <a href="/Particular/docs.particular.net/pulse" aria-label="Pulse" class="js-selected-navigation-item sunken-menu-item" data-pjax="true" data-selected-links="pulse /Particular/docs.particular.net/pulse">
          <span class="octicon octicon-pulse"></span> <span class="full-word">Pulse</span>
          <img alt="Octocat-spinner-32" class="mini-loader" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
</a>      </li>

      <li class="tooltipped leftwards" title="Graphs">
        <a href="/Particular/docs.particular.net/graphs" aria-label="Graphs" class="js-selected-navigation-item sunken-menu-item" data-pjax="true" data-selected-links="repo_graphs repo_contributors /Particular/docs.particular.net/graphs">
          <span class="octicon octicon-graph"></span> <span class="full-word">Graphs</span>
          <img alt="Octocat-spinner-32" class="mini-loader" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
</a>      </li>

      <li class="tooltipped leftwards" title="Network">
        <a href="/Particular/docs.particular.net/network" aria-label="Network" class="js-selected-navigation-item sunken-menu-item js-disable-pjax" data-selected-links="repo_network /Particular/docs.particular.net/network">
          <span class="octicon octicon-git-branch"></span> <span class="full-word">Network</span>
          <img alt="Octocat-spinner-32" class="mini-loader" height="16" src="https://github.global.ssl.fastly.net/images/spinners/octocat-spinner-32.gif" width="16" />
</a>      </li>
    </ul>


  </div>
</div>

              <div class="only-with-full-nav">
                

  

<div class="clone-url "
  data-protocol-type="http"
  data-url="/users/set_protocol?protocol_selector=http&amp;protocol_type=push">
  <h3><strong>HTTPS</strong> clone URL</h3>
  <div class="clone-url-box">
    <input type="text" class="clone js-url-field"
           value="https://github.com/Particular/docs.particular.net.git" readonly="readonly">

    <span class="js-zeroclipboard url-box-clippy minibutton zeroclipboard-button" data-clipboard-text="https://github.com/Particular/docs.particular.net.git" data-copied-hint="copied!" title="copy to clipboard"><span class="octicon octicon-clippy"></span></span>
  </div>
</div>

  

<div class="clone-url open"
  data-protocol-type="ssh"
  data-url="/users/set_protocol?protocol_selector=ssh&amp;protocol_type=push">
  <h3><strong>SSH</strong> clone URL</h3>
  <div class="clone-url-box">
    <input type="text" class="clone js-url-field"
           value="git@github.com:Particular/docs.particular.net.git" readonly="readonly">

    <span class="js-zeroclipboard url-box-clippy minibutton zeroclipboard-button" data-clipboard-text="git@github.com:Particular/docs.particular.net.git" data-copied-hint="copied!" title="copy to clipboard"><span class="octicon octicon-clippy"></span></span>
  </div>
</div>

  

<div class="clone-url "
  data-protocol-type="subversion"
  data-url="/users/set_protocol?protocol_selector=subversion&amp;protocol_type=push">
  <h3><strong>Subversion</strong> checkout URL</h3>
  <div class="clone-url-box">
    <input type="text" class="clone js-url-field"
           value="https://github.com/Particular/docs.particular.net" readonly="readonly">

    <span class="js-zeroclipboard url-box-clippy minibutton zeroclipboard-button" data-clipboard-text="https://github.com/Particular/docs.particular.net" data-copied-hint="copied!" title="copy to clipboard"><span class="octicon octicon-clippy"></span></span>
  </div>
</div>


<p class="clone-options">You can clone with
      <a href="#" class="js-clone-selector" data-protocol="http">HTTPS</a>,
      <a href="#" class="js-clone-selector" data-protocol="ssh">SSH</a>,
      or <a href="#" class="js-clone-selector" data-protocol="subversion">Subversion</a>.
  <span class="octicon help tooltipped upwards" title="Get help on which URL is right for you.">
    <a href="https://help.github.com/articles/which-remote-url-should-i-use">
    <span class="octicon octicon-question"></span>
    </a>
  </span>
</p>


  <a href="github-windows://openRepo/https://github.com/Particular/docs.particular.net" class="minibutton sidebar-button">
    <span class="octicon octicon-device-desktop"></span>
    Clone in Desktop
  </a>

                <a href="/Particular/docs.particular.net/archive/Drafts.zip"
                   class="minibutton sidebar-button"
                   title="Download this repository as a zip file"
                   rel="nofollow">
                  <span class="octicon octicon-cloud-download"></span>
                  Download ZIP
                </a>
              </div>
        </div><!-- /.repository-sidebar -->

        <div id="js-repo-pjax-container" class="repository-content context-loader-container" data-pjax-container>
          


<!-- blob contrib key: blob_contributors:v21:9e61179d29ad4c49cfdb9a4ede15e562 -->

<p title="This is a placeholder element" class="js-history-link-replace hidden"></p>

<a href="/Particular/docs.particular.net/find/Drafts" data-pjax data-hotkey="t" class="js-show-file-finder" style="display:none">Show File Finder</a>

<div class="file-navigation">
  

<div class="select-menu js-menu-container js-select-menu" >
  <span class="minibutton select-menu-button js-menu-target" data-hotkey="w"
    data-master-branch="master"
    data-ref="Drafts"
    role="button" aria-label="Switch branches or tags" tabindex="0">
    <span class="octicon octicon-git-branch"></span>
    <i>branch:</i>
    <span class="js-select-button">Drafts</span>
  </span>

  <div class="select-menu-modal-holder js-menu-content js-navigation-container" data-pjax>

    <div class="select-menu-modal">
      <div class="select-menu-header">
        <span class="select-menu-title">Switch branches/tags</span>
        <span class="octicon octicon-remove-close js-menu-close"></span>
      </div> <!-- /.select-menu-header -->

      <div class="select-menu-filters">
        <div class="select-menu-text-filter">
          <input type="text" aria-label="Find or create a branch…" id="context-commitish-filter-field" class="js-filterable-field js-navigation-enable" placeholder="Find or create a branch…">
        </div>
        <div class="select-menu-tabs">
          <ul>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="branches" class="js-select-menu-tab">Branches</a>
            </li>
            <li class="select-menu-tab">
              <a href="#" data-tab-filter="tags" class="js-select-menu-tab">Tags</a>
            </li>
          </ul>
        </div><!-- /.select-menu-tabs -->
      </div><!-- /.select-menu-filters -->

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="branches">

        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


            <div class="select-menu-item js-navigation-item selected">
              <span class="select-menu-item-icon octicon octicon-check"></span>
              <a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md"
                 data-name="Drafts"
                 data-skip-pjax="true"
                 rel="nofollow"
                 class="js-navigation-open select-menu-item-text js-select-button-text css-truncate-target"
                 title="Drafts">Drafts</a>
            </div> <!-- /.select-menu-item -->
            <div class="select-menu-item js-navigation-item ">
              <span class="select-menu-item-icon octicon octicon-check"></span>
              <a href="/Particular/docs.particular.net/blob/master/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md"
                 data-name="master"
                 data-skip-pjax="true"
                 rel="nofollow"
                 class="js-navigation-open select-menu-item-text js-select-button-text css-truncate-target"
                 title="master">master</a>
            </div> <!-- /.select-menu-item -->
            <div class="select-menu-item js-navigation-item ">
              <span class="select-menu-item-icon octicon octicon-check"></span>
              <a href="/Particular/docs.particular.net/blob/platform/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md"
                 data-name="platform"
                 data-skip-pjax="true"
                 rel="nofollow"
                 class="js-navigation-open select-menu-item-text js-select-button-text css-truncate-target"
                 title="platform">platform</a>
            </div> <!-- /.select-menu-item -->
        </div>

          <form accept-charset="UTF-8" action="/Particular/docs.particular.net/branches" class="js-create-branch select-menu-item select-menu-new-item-form js-navigation-item js-new-item-form" method="post"><div style="margin:0;padding:0;display:inline"><input name="authenticity_token" type="hidden" value="nSnsVcwkFZlDqM/hN6JQ+AXRziKR4KREnCV1OH2OoCE=" /></div>
            <span class="octicon octicon-git-branch-create select-menu-item-icon"></span>
            <div class="select-menu-item-text">
              <h4>Create branch: <span class="js-new-item-name"></span></h4>
              <span class="description">from ‘Drafts’</span>
            </div>
            <input type="hidden" name="name" id="name" class="js-new-item-value">
            <input type="hidden" name="branch" id="branch" value="Drafts" />
            <input type="hidden" name="path" id="path" value="Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md" />
          </form> <!-- /.select-menu-item -->

      </div> <!-- /.select-menu-list -->

      <div class="select-menu-list select-menu-tab-bucket js-select-menu-tab-bucket" data-tab-filter="tags">
        <div data-filterable-for="context-commitish-filter-field" data-filterable-type="substring">


        </div>

        <div class="select-menu-no-results">Nothing to show</div>
      </div> <!-- /.select-menu-list -->

    </div> <!-- /.select-menu-modal -->
  </div> <!-- /.select-menu-modal-holder -->
</div> <!-- /.select-menu -->

  <div class="breadcrumb">
    <span class='repo-root js-repo-root'><span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/Particular/docs.particular.net/tree/Drafts" data-branch="Drafts" data-direction="back" data-pjax="true" itemscope="url"><span itemprop="title">docs.particular.net</span></a></span></span><span class="separator"> / </span><span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/Particular/docs.particular.net/tree/Drafts/Content" data-branch="Drafts" data-direction="back" data-pjax="true" itemscope="url"><span itemprop="title">Content</span></a></span><span class="separator"> / </span><span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/Particular/docs.particular.net/tree/Drafts/Content/NServiceBus" data-branch="Drafts" data-direction="back" data-pjax="true" itemscope="url"><span itemprop="title">NServiceBus</span></a></span><span class="separator"> / </span><strong class="final-path">getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md</strong> <span class="js-zeroclipboard minibutton zeroclipboard-button" data-clipboard-text="Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md" data-copied-hint="copied!" title="copy to clipboard"><span class="octicon octicon-clippy"></span></span>
  </div>
</div>


  <div class="commit file-history-tease">
    <img alt="Sean Farmar" class="main-avatar" height="24" src="https://0.gravatar.com/avatar/9a7cc5428cf85260083b765f2616146c?d=https%3A%2F%2Fidenticons.github.com%2F132d16811e7db45a5c501541c275851c.png&amp;r=x&amp;s=140" width="24" />
    <span class="author"><a href="/sfarmar" rel="author">sfarmar</a></span>
    <time class="js-relative-date" datetime="2014-01-19T07:58:50-08:00" title="2014-01-19 07:58:50">January 19, 2014</time>
    <div class="commit-title">
        <a href="/Particular/docs.particular.net/commit/ebd062220b7d6f50356c99ef4285c91f1a840b57" class="message" data-pjax="true" title="updated drafts export_drafts_201401191547">updated drafts export_drafts_201401191547</a>
    </div>

    <div class="participation">
      <p class="quickstat"><a href="#blob_contributors_box" rel="facebox"><strong>1</strong> contributor</a></p>
      
    </div>
    <div id="blob_contributors_box" style="display:none">
      <h2 class="facebox-header">Users who have contributed to this file</h2>
      <ul class="facebox-user-list">
          <li class="facebox-user-list-item">
            <img alt="Sean Farmar" height="24" src="https://0.gravatar.com/avatar/9a7cc5428cf85260083b765f2616146c?d=https%3A%2F%2Fidenticons.github.com%2F132d16811e7db45a5c501541c275851c.png&amp;r=x&amp;s=140" width="24" />
            <a href="/sfarmar">sfarmar</a>
          </li>
      </ul>
    </div>
  </div>

<div id="files" class="bubble">
  <div class="file">
    <div class="meta">
      <div class="info">
        <span class="icon"><b class="octicon octicon-file-text"></b></span>
        <span class="mode" title="File Mode">file</span>
          <span>121 lines (81 sloc)</span>
        <span>6.219 kb</span>
      </div>
      <div class="actions">
        <div class="button-group">
            <a class="minibutton tooltipped leftwards"
               href="github-windows://openRepo/https://github.com/Particular/docs.particular.net?branch=Drafts&amp;filepath=Content%2FNServiceBus%2Fgetting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md" title="Open this file in GitHub for Windows">
                <span class="octicon octicon-device-desktop"></span> Open
            </a>
                <a class="minibutton js-update-url-with-hash"
                   href="/Particular/docs.particular.net/edit/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md"
                   data-method="post" rel="nofollow" data-hotkey="e">Edit</a>
          <a href="/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md" class="button minibutton " id="raw-url">Raw</a>
            <a href="/Particular/docs.particular.net/blame/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md" class="button minibutton js-update-url-with-hash">Blame</a>
          <a href="/Particular/docs.particular.net/commits/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md" class="button minibutton " rel="nofollow">History</a>
        </div><!-- /.button-group -->
          <a class="minibutton danger empty-icon tooltipped downwards"
             href="/Particular/docs.particular.net/delete/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance.md"
             title=""
             data-method="post" data-test-id="delete-blob-file" rel="nofollow">
          Delete
        </a>
      </div><!-- /.actions -->
    </div>
      
  <div id="readme" class="blob instapaper_body">
    <article class="markdown-body entry-content" itemprop="mainContentOfPage"><table data-table-type="yaml-metadata">
<thead><tr>
<th>title</th>

  <th>summary</th>

  <th>originalUrl</th>

  <th>tags</th>

  <th>createdDate</th>

  <th>modifiedDate</th>

  <th>authors</th>

  <th>reviewers</th>

  <th>contributors</th>
  </tr></thead>
<tbody><tr>
<td><div>Getting Started with NServiceBus using ServiceMatrix v2.0 - Fault Tolerance</div></td>

  <td><div></div></td>

  <td><div><a href="http://www.particular.net/articles/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance">http://www.particular.net/articles/getting-started-with-nservicebus-using-servicematrix-2.0---fault-tolerance</a></div></td>

  <td><div><table><tbody></tbody></table></div></td>

  <td><div>2013-09-04 09:56:45 UTC</div></td>

  <td><div>2013-09-04 15:11:09 UTC</div></td>

  <td><div><table><tbody></tbody></table></div></td>

  <td><div><table><tbody></tbody></table></div></td>

  <td><div><table><tbody></tbody></table></div></td>
  </tr></tbody>
</table><h2>
<a name="durable-messaging" class="anchor" href="#durable-messaging"><span class="octicon octicon-link"></span></a>Durable messaging</h2>

<p>In <a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/getting-started-with-servicematrix.md">the previous section</a> you've seen how a web application can send messages to a console application, see how messaging can get past all sorts of failure scenarios:</p>

<ol>
<li> Run the solution again to make sure the messages are being
processed.</li>
<li> Then, kill the OrderProcessing endpoint but leave the web
application running.</li>
<li> Click the "About" link a couple more times to see that the web
application isn't blocked even when the other process it's trying to
communicate with is down. This makes it easier to upgrade the
backend even while the front-end is still running, resulting in a
more highly-available system.</li>
<li> Now, leaving the web application running, go back to Visual Studio
and open Server Explorer. You should see this:</li>
</ol><p><a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/GettingStarted8.jpg" target="_blank"><img src="/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/GettingStarted8.jpg" alt="Server Explorer" style="max-width:100%;"></a></p>

<p>All the messages sent to the OrderProcessing endpoint are queued, waiting for the process to come back online. You can click each message, press F4, and examine its properties specifically BodyStream, where the data is.</p>

<p>Now bring the OrderProcessing endpoint back online by right clicking the project, Debug, Start new instance. It processes all those messages, and if you go back to the queue shown above and right click Refresh, it is empty.</p>

<h2>
<a name="fault-tolerance" class="anchor" href="#fault-tolerance"><span class="octicon octicon-link"></span></a>Fault tolerance</h2>

<p>Consider scenarios where the processing of a message fails. This could be due to something transient like a deadlock in the database, in which case some quick retries overcome this problem, making the message processing ultimately succeed. NServiceBus automatically retries immediately when an exception is thrown during message processing, up to five times by default (which is configurable).</p>

<p>If the problem is something more protracted, like a third party web service going down or a database being unavailable, it makes sense to try again some time later. This is called the " <a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/second-level-retries">second level retries</a> " functionality of NServiceBus. Configure its behavior by selecting the OrderProcessing endpoint in Solution Builder and opening its properties (F4). </p>

<p><a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/GettingStarted8.5.jpg" target="_blank"><img src="/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/GettingStarted8.5.jpg" alt="Endpoint properties" style="max-width:100%;"></a></p>

<p>You will get to the Error Queue and other General properties later. For now, focus on the SecondLevelRetriesConfig section.</p>

<ul>
<li>  The <strong>Enabled</strong> attribute defines whether this endpoint performs the
second-level retry logic.</li>
<li>  The <strong>NumberOfRetries</strong> attribute defines how many times a message
has its processing attempted again. The TimeIncrease attribute
defines how much time is added on each attempt.</li>
<li>  The defaults are three retries and ten seconds, resulting in a wait
of 10s, then 20s, and then 30s; after which the message moves to the
defined <strong>ErrorQueue</strong> .</li>
</ul><p>So, make the processing of messages in OrderProcessing fail. Throw an exception in the SubmitOrderProcessor code like this:</p>

<div class="highlight highlight-C#"><pre><span class="k">using</span> <span class="nn">System</span><span class="p">;</span>
<span class="k">using</span> <span class="nn">NServiceBus</span><span class="p">;</span>
<span class="k">using</span> <span class="nn">Amazon.InternalMessages.Sales</span><span class="p">;</span>

<span class="k">namespace</span> <span class="nn">Amazon.OrderProcessing.Sales</span>
<span class="p">{</span>
    <span class="k">public</span> <span class="k">partial</span> <span class="k">class</span> <span class="nc">SubmitOrderProcessor</span>
    <span class="p">{</span>
        <span class="k">partial</span> <span class="k">void</span> <span class="nf">HandleImplementation</span><span class="p">(</span><span class="n">SubmitOrder</span> <span class="n">message</span><span class="p">)</span>
        <span class="p">{</span>
            <span class="n">Console</span><span class="p">.</span><span class="n">WriteLine</span><span class="p">(</span><span class="s">"Sales received "</span> <span class="p">+</span> <span class="n">message</span><span class="p">.</span><span class="n">GetType</span><span class="p">().</span><span class="n">Name</span><span class="p">);</span>

            <span class="k">throw</span> <span class="k">new</span> <span class="nf">Exception</span><span class="p">(</span><span class="s">"Uh oh - a bug."</span><span class="p">);</span>
        <span class="p">}</span>
    <span class="p">}</span>
<span class="p">}</span>
</pre></div>

<p>Run your solution again, but this time use Ctrl-F5 so that Visual Studio does not break each time the exception is thrown, sending a message from the ECommerce app by clicking About. You should see the endpoint scroll a bunch of warnings, ultimately putting out an error, and stopping, like this:</p>

<p><a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/GettingStarted9.png" target="_blank"><img src="/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/GettingStarted9.png" alt="Retries" style="max-width:100%;"></a></p>

<p>While the endpoint can now continue processing other incoming messages
(which will also fail in this case as the exception is thrown for all cases), the failed message has been diverted and is being held in one of the NServiceBus internal databases.</p>

<p>If you leave the endpoint running a while longer, you'll see that it tries processing the message again. After three retries, the retries stop and the message ends up in the error queue (in the default configuration this should be after roughly one minute).</p>

<p><strong>NOTE</strong> : When a message cannot be deserialized, it bypasses all retry behaviors and moves directly to the error queue.</p>

<h2>
<a name="retries-errors-and-auditing" class="anchor" href="#retries-errors-and-auditing"><span class="octicon octicon-link"></span></a>Retries, errors, and auditing</h2>

<p>If a message fails continuously (due to a bug in the system, for example), it ultimately moves to the error queue that is configured for the endpoint after all the various retries have been performed.</p>

<p>Since administrators must monitor these error queues, it is recommended that all endpoints use the same error queue. You can set the error queue for all endpoints via the properties of your top level design element in this case, Amazon. Press F4 to see the properties window. </p>

<p><a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/GettingStarted10.png" target="_blank"><img src="/Particular/docs.particular.net/raw/Drafts/Content/NServiceBus/GettingStarted10.png" alt="System level queue configuration" style="max-width:100%;"></a></p>

<p><strong>NOTE</strong> : If you specify an error queue for a specific endpoint, it won't change when you change the top level error queue. Changing the</p>

<p>top-level error queue only sets the value for endpoints for whom you haven't modified the error queue.</p>

<p>The second value, ForwardReceivedMessagesTo, defines the queue to which all messages will be audited. Any message that is processed by an endpoint is forwarded to this queue. This too can be overridden per endpoint.</p>

<p>In production, set both of these queues to be on a central machine by setting a value like "error@machine" or "error@IP-Address". Read about
<a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/second-level-retries">how to configure retries</a> .</p>

<p>Make sure you remove the code which throws an exception before going on.</p>

<h2>
<a name="next-steps" class="anchor" href="#next-steps"><span class="octicon octicon-link"></span></a>Next steps</h2>

<p>See how to use NServiceBus for
<a href="/Particular/docs.particular.net/blob/Drafts/Content/NServiceBus/getting-started-with-nservicebus-using-servicematrix-2.0---publish-subscribe.md">Publish/Subscribe</a>
.</p></article>
  </div>

  </div>
</div>

<a href="#jump-to-line" rel="facebox[.linejump]" data-hotkey="l" class="js-jump-to-line" style="display:none">Jump to Line</a>
<div id="jump-to-line" style="display:none">
  <form accept-charset="UTF-8" class="js-jump-to-line-form">
    <input class="linejump-input js-jump-to-line-field" type="text" placeholder="Jump to line&hellip;" autofocus>
    <button type="submit" class="button">Go</button>
  </form>
</div>

        </div>

      </div><!-- /.repo-container -->
      <div class="modal-backdrop"></div>
    </div><!-- /.container -->
  </div><!-- /.site -->


    </div><!-- /.wrapper -->

      <div class="container">
  <div class="site-footer">
    <ul class="site-footer-links right">
      <li><a href="https://status.github.com/">Status</a></li>
      <li><a href="http://developer.github.com">API</a></li>
      <li><a href="http://training.github.com">Training</a></li>
      <li><a href="http://shop.github.com">Shop</a></li>
      <li><a href="/blog">Blog</a></li>
      <li><a href="/about">About</a></li>

    </ul>

    <a href="/">
      <span class="mega-octicon octicon-mark-github" title="GitHub"></span>
    </a>

    <ul class="site-footer-links">
      <li>&copy; 2014 <span title="0.03391s from github-fe132-cp1-prd.iad.github.net">GitHub</span>, Inc.</li>
        <li><a href="/site/terms">Terms</a></li>
        <li><a href="/site/privacy">Privacy</a></li>
        <li><a href="/security">Security</a></li>
        <li><a href="/contact">Contact</a></li>
    </ul>
  </div><!-- /.site-footer -->
</div><!-- /.container -->


    <div class="fullscreen-overlay js-fullscreen-overlay" id="fullscreen_overlay">
  <div class="fullscreen-container js-fullscreen-container">
    <div class="textarea-wrap">
      <textarea name="fullscreen-contents" id="fullscreen-contents" class="js-fullscreen-contents" placeholder="" data-suggester="fullscreen_suggester"></textarea>
          <div class="suggester-container">
              <div class="suggester fullscreen-suggester js-navigation-container" id="fullscreen_suggester"
                 data-url="/Particular/docs.particular.net/suggestions/commit">
              </div>
          </div>
    </div>
  </div>
  <div class="fullscreen-sidebar">
    <a href="#" class="exit-fullscreen js-exit-fullscreen tooltipped leftwards" title="Exit Zen Mode">
      <span class="mega-octicon octicon-screen-normal"></span>
    </a>
    <a href="#" class="theme-switcher js-theme-switcher tooltipped leftwards"
      title="Switch themes">
      <span class="octicon octicon-color-mode"></span>
    </a>
  </div>
</div>



    <div id="ajax-error-message" class="flash flash-error">
      <span class="octicon octicon-alert"></span>
      <a href="#" class="octicon octicon-remove-close close js-ajax-error-dismiss"></a>
      Something went wrong with that request. Please try again.
    </div>

  </body>
</html>

