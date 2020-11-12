<style>
.productlink {
    font-size: 24px;
    font-weight: bold;
    color: black;
    margin: 8px;
    display: inline-block;
}
.block.black a,
.block.middle a,
.productcolumn a {
  color: inherit;
}
.block {
  width: 100%;
  background-color: rgb(233, 240, 242);
  padding: 21px;
  margin-bottom: 2px;
  display: inline-block;
}
.block.top img,
.productcolumn img {
  float: left;
}
.block.black img {
  float: inherit;
}
.block.middle .ic {
  min-width: 25%;
  float: left;
  text-align: center;
  font-size: 18px;
  font-weight: bold;
  color: rgb(0, 114, 156);
}
.block.black {
  margin-top: -2px;
  margin-bottom: 0px;
  width: 100%;
  clear: both;
  background-color: rgb(26, 26, 26);
  font-size: 16px;
  font-weight: bold;
  padding-top: 13px;
  padding-bottom: 13px;
  line-height: 30px;
}
span.blue {
  color: rgb(0, 191, 230);
  padding-right: 30px;
  display: inline-block;
}
.block.black span img {
  padding-left: 0px;
  padding-right: 5px;
  margin-top: -3px;
}
.productcolumn .black {
  font-size: 14px;
}
.block h3 {
  font-weight: bold;
  font-size: 17px !important;
  margin-top: 0px;
  margin-bottom: 0px;
  color: rgb(0, 114, 156);
}
.block h4 {
  font-size: 16px !important;
  font-weight: bold;
  margin-top: 0px;
}
.block p {
  font-size: 14px;
  color: rgb(77, 77, 77);
}

.block.resources .col-xs-6 img,
.block.resources .col-sm-6 img,
.block.resources .col-md-6 img {
  float: left;
  margin: 0px 13px 0px 0px;
}
.productcolumn {
  margin-right: 2%;
  float: left;
}
.productcolumn.header {
  margin-top: 2%;
}
.productcolumn.last {
  margin-right: 0px;
}
.productcolumnc {
  overflow: hidden;
  clear: both;
}
.productcolumnc .productcolumn {
  padding-bottom: 1000px;
  margin-bottom: -1000px;
}
.productcolumnc ul {
  list-style: none;
  margin-left: 0px;
  padding-left: 0px;
}
.productcolumnc li {
  color: rgb(0, 114, 156) !important;
  font-size: 14px;
  font-weight: bold;
  padding-bottom: 7px;
}
.mainicon{
  font-size: 67px;
}
#new-user-quickstart-alert {
    display: none !important;
}

.products {
  margin-top: 30px;
}

.products .col-xs-12.col-md-4 {
  padding: 0;
}

.products .col-xs-12.col-md-4:nth-child(2) {
  padding: 0 15px;
}

.products .col-xs-12.col-md-4 ul {
  padding-left: 15px;
}

.products .col-xs-12.col-md-4 ul a {
  color: rgb(0, 114, 156);
  font-weight: bold;
}

.productcolumn {
  height: 230px;
}

@media (max-width: 992px) {

  .block.middle {
    padding-bottom: 0;
  }

  .block.middle .ic {
    margin-bottom: 40px;
  }

  .col-md-12.block.resources {
    padding-bottom: 0;
  }

  .resources .col-xs-6, .resources .col-sm-6 {
    margin-bottom: 30px;
  }
  
  .resources .col-xs-6 p, .resources .col-sm-6 p, .resources .col-xs-6 h3, .resources .col-sm-6 h3 {
    padding-left: 50px;
  }
  .products .col-xs-12.col-md-4 {
    margin-bottom: 20px;
  }
  .products .col-xs-12.col-md-4:nth-child(2) {
    padding: 0;
  }
  .productcolumn {
    height: initial;
  }
}

@media (max-width: 517px) {
  span.pull-right {
    float: initial !important;    
  }

  span.pull-right a {
    margin-top: 20px;
    width: 100%;
  }
}
</style>
<div class="row">
<div class="col-md-12 block top clearfix">
  <a href="/nservicebus/"><img src="/content/images/menu/nservicebus-icon.svg" width="47" height="47"><span class="productlink">NServiceBus</span></a>
  <span class="pull-right">
    <a class="btn btn-info btn-lg hidden-sm hidden-xs" href="https://github.com/Particular/docs.particular.net/issues/new" target="_blank"><em class="glyphicon glyphicon-comment"></em> Feedback</a>
    <a type="button" class="btn btn-primary btn-lg" href="/nservicebus/">Documentation topics</a>
  </span>
</div>
</div>
<div class="row">
<div class="block middle">
  <div class="ic col-xs-12 col-sm-6 col-md-3">
    <a href="/get-started/" onclick="ga('send', 'event', 'Action Performed', 'Clicked Get-Started CTA (Docs Home, Direct to get-started)'); return true">
      <img src="/content/images/menu/getting-started-icon.svg" height="64" width="64" style="margin: 4px 0;" /><br>Getting Started
    </a>
  </div>
  <div class="ic col-xs-12 col-sm-6 col-md-3">
    <a href="https://particular.net/videos">
      <i class="glyphicon glyphicon-film mainicon"></i><br>Intro Videos
    </a>
  </div>
  <div class="ic col-xs-12 col-sm-6 col-md-3">
    <a href="https://discuss.particular.net">
      <img src="/content/images/discussiongroup_v1.svg" style="height:72px; width: 110px; margin-bottom: 1px;"><br>Discussion group
    </a>
  </div>
  <div class="ic col-xs-12 col-sm-6 col-md-3">
    <a href="/samples/" class="rarr">
      <img src="/content/images/menu/samples-icon.svg" height="72" width="72"/><br>Samples
    </a>
  </div>
</div>
</div>
<div class="row">
<div class="col-md-12 block black">
  <span class="blue"><a href="https://github.com/Particular/NServiceBus/releases"><span class="glyphicon glyphicon-calendar"></span> Release notes</a></span>
  <span class="blue">
<a href="https://particular.net/downloads"><span class="glyphicon glyphicon-download"></span> Downloads</a>
</span>
</div>
</div>
<div class="row">
<div class="col-md-12 block resources">
  <div class="row">
    <div class="col-xs-12 col-sm-6">
      <a href="/platform/">
        <img src="/content/images/particular_v1.svg" width="36" height="36"/>
        <h3>Service Platform Overview</h3>
      </a>
      <p>A short and high level overview of the Platform</p>
    </div>
    <div class="col-xs-12 col-sm-6">
      <a href="https://www.pluralsight.com/courses/microservices-nservicebus6-scaling-applications">
        <img src="/home/pluralsight_v1.svg" width="36" height="36">
        <h3>Pluralsight: Scaling Applications with Microservices and NServiceBus 6</h3>
      </a>
    </div>
  </div>
  <div class="row">
    <div class="col-xs-12 col-sm-6">
      <a href="https://stackoverflow.com/questions/tagged/nservicebus">
        <img src="/home/stackoverflow_v1.svg" width="36" height="36">
        <h3>StackOverflow</h3>
      </a>
      <p>All questions and answers tagged 'NServiceBus'</p>
    </div>
    <div class="col-xs-12 col-sm-6">
      <a href="/nservicebus/community/">
        <img src="/home/extensions_v1.svg" width="36" height="36">
        <h3>Extensions and integrations</h3>
      </a>
      <p>Extensions developed by the community</p>
    </div>
  </div>
</div>
</div>


<div class="row products">

  <div class="col-xs-12 col-md-4">
    <div class="block top">
      <a href="/serviceinsight/"><img src="/content/images/menu/serviceinsight-icon.svg" width="47" height="47"><span class="productlink">ServiceInsight</span></a>
      <div style="clear: both"></div>
    </div>
    <div class="productcolumn block">
      <p></p>
      <h4>Complete under-the-hood visualization of a system's behavior</h4>
      <p></p>
      <ul>
        <li><a href="/serviceinsight/application-invocation.md">Application invocation</a></li>
        <li><a href="/serviceinsight/logging.md">Logging</a></li>
      </ul>
      <a href="/serviceinsight/"><h3>All ServiceInsight articles</h3></a><br/>
      <div style="clear: both"></div>
    </div>
    <div class="block black">
      <span class="blue"><a href="https://github.com/Particular/ServiceInsight/releases"><span class="glyphicon glyphicon-calendar"></span> Release notes</a></span>
    </div>
  </div>


  <div class="col-xs-12 col-md-4">
    <div class="block top">
      <a href="/servicecontrol/"><img src="/content/images/menu/servicecontrol-icon.svg" width="47" height="47"><span class="productlink">ServiceControl</span></a>
      <div style="clear: both"></div>
    </div>
    <div class="productcolumn block">
      <p></p>
      <h4>Gather all the data and monitor multiple endpoints from a single location</h4>
      <p></p>
      <ul>
        <li><a href="/servicecontrol/servicecontrol-in-practice.md">Optimizing use in different environments</a></li>
        <li><a href="/servicecontrol/installation.md">Installing</a></li>
      </ul>
      <a href="/servicecontrol/"><h3>All ServiceControl articles</h3></a><br/>
      <div style="clear: both"></div>
    </div>
    <div class="block black">
      <span class="blue"><a href="https://github.com/Particular/ServiceControl/releases"><span class="glyphicon glyphicon-calendar"></span> Release notes</a></span>
    </div>
  </div>




  <div class="col-xs-12 col-md-4">
    <div class="block top">
      <a href="/servicepulse/"><img src="/content/images/menu/servicepulse-icon.svg" width="47" height="47"><span class="productlink">ServicePulse</span></a>
      <div style="clear: both"></div>
    </div>
    <div class="productcolumn last block">
      <p></p>
      <h4>Real-time monitoring that is custom tailored to fit a distributed systems</h4>
      <p></p>
      <ul>
        <li><a href="/monitoring/metrics/in-servicepulse.md">Monitoring Endpoints</a></li>
        <li><a href="/servicepulse/intro-failed-messages.md">Handling Failed Messages</a></li>
      </ul>
      <a href="/servicepulse/"><h3>All ServicePulse articles</h3></a><br/>
      <div style="clear: both"></div>
    </div>
    <div class="block black">
      <span class="blue"><a href="https://github.com/Particular/ServicePulse/releases"><span class="glyphicon glyphicon-calendar"></span> Release notes</a></span>
    </div>
  </div>

</div>
