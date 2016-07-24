<style>
.small.button {
  line-height: 45px;
  font-size: 16px;
  padding-left: 15px;
  font-family: 'Lato', Bold;
  display: inline-block;
}
.small.button a {
  display: block;
  color: white;
  line-height: 45px;
  width: 215px;
  background-color: #00a3c4;
  border-bottom: 5px solid #0071a0;
  clear: both;
  text-align: center;
  text-transform: uppercase;
  text-decoration: none;
  font-weight: 700;
}
.small.button a:hover {
  background-color: #03AFF8;
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
  font-family: 'Lato';
  display: inline-block;
}
.block.top img,
.productcolumn img {
  float: left;
}
.block.black img {
  float: inherit;
}
.block.top .button {
  float: right;
  width: 225px;
  font-size: 15px;
}
.block.middle .ic {
  min-width: 25%;
  float: left;
  text-align: center;
  font-size: 18px;
  font-weight: bold;
  line-height: 50px;
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
  color: rgb(0, 163, 196);
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
  font-size: 17px;
  margin-top: 0px;
  margin-bottom: 0px;
  color: rgb(0, 114, 156);
}
.block h4 {
  font-size: 16px !important;
  font-family: 'Dosis', bold;
  font-weight: bold;
  margin-top: 0px;
}
.block p {
  font-size: 14px;
  color: rgb(77, 77, 77);
}
.block .col-md-6 img,
.block .col-md-6 img {
  float: left;
  margin: 0px 13px 0px 0px;
}
.productcolumn {
  width: 32%;
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
</style>
<div class="row">
<div class="col-md-12 block top">
  <a href="/nservicebus/"><img src="/home/nservicebus_v1.svg"  width="208" height="47"></a>
  <div class="small button">
    <a class="blue" href="/nservicebus/">Documentation topics</a>
  </div>
</div>
</div>
<div class="row">
<div class="col-md-12 block middle">
  <div class="ic">
    <a href="/samples/step-by-step/">
      <img src="/home/getting-started_v1.svg" height="72" width="72"><br>Getting Started
    </a>
  </div>
  <div class="ic">
    <a href="http://particular.net/videos-and-presentations">
      <img src="/home/videos_v1.svg" height="72" width="72"><br>Intro Videos
    </a>
  </div>
  <div class="ic">
    <a href="https://groups.google.com/forum/#!forum/particularsoftware">
      <img src="/home/discussion_v1.svg" height="72" width="72"/><br/>Discussion group
    </a>
  </div>
  <div class="ic">
    <a href="/samples/" class="rarr">
      <img src="/home/samples_v1.svg" height="72" width="72"/><br/>Samples
    </a>
  </div>
</div>
</div>
<div class="row">
<div class="col-md-12 block black">
  <span class="blue"><a href="https://github.com/Particular/NServiceBus/releases"><img src="/home/release-notes_v1.svg" height="16"> Release notes</a></span>
  <span class="blue">
<a href="http://particular.net/downloads"><img src="/home/download_v1.svg" height="16"> Downloads</a>
</span>
</div>
</div>
<div class="row">
<div class="col-md-12 block ">
  <div class="row">
    <div class="col-md-6">
      <a href="/platform/">
        <img src="/home/platform_v1.svg" width="36" height="36"/>
        <h3>Particular Service Platform Overview</h3>
      </a>
      <p>A short and high level overview of the Platform</p>
    </div>
    <div class="col-md-6">
      <a href="https://www.pluralsight.com/courses/microservices-nservicebus-scaling-applications">
        <img src="/home/pluralsight_v1.svg" width="36" height="36">
        <h3>Pluralsight: Scaling Applications with<br> Microservices and NServiceBus</h3>
      </a>
    </div>
  </div>
  <div class="row">
    <div class="col-md-6">
      <a href="https://www.packtpub.com/application-development/learning-nservicebus-second-edition">
        <img src="/home/book_v1.svg" width="36" height="36">
        <h3>Learning NServiceBus - Second Edition</h3>
      </a>
      <p>Building reliable and scalable software with NServiceBus.</p>
    </div>
    <div class="col-md-6">
      <a href="http://stackoverflow.com/questions/tagged/nservicebus">
        <img src="/home/stackoverflow_v1.svg" width="36" height="36">
        <h3>StackOverflow</h3>
      </a>
      <p>All questions and answers tagged 'NServiceBus'</p>
    </div>
  </div>
  <div class="row">
    <div class="col-md-6">
      <a href="/platform/extensions.md">
        <img src="/home/extensions_v1.svg" width="36" height="36">
        <h3>Extensions and integrations</h3>
      </a>
      <p>Extensions developed by both the community and Particular</p>
    </div>
    <div class="col-md-6">
    </div>
  </div>
</div>
</div>
<div class="row">
<div class="productcolumn header">
  <div class="block top">
    <a href="/serviceinsight/">
      <img src="/home/serviceinsight_v1.svg" width="219" height="47"/>
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn header">
  <div class="block top">
    <a href="/servicecontrol/">
      <img src="/home/servicecontrol_v1.svg" width="219" height="47"/>
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn header last">
  <div class="block top">
    <a href="/servicepulse/">
      <img src="/home/servicepulse_v1.svg" width="219" height="47"/>
    </a>
    <div style="clear: both"></div>
  </div>
</div>
</div>
<div class="row">
<div class="productcolumnc">
  <div class="productcolumn block">
    <p></p>
    <h4>Complete under-the-hood visualization of a system's behavior</h4>
    <p></p>
    <ul>
      <li><a href="/serviceinsight/application-invocation.md">Application invocation</a></li>
      <li><a href="/serviceinsight/logging.md">Logging in ServiceInsight</a></li>
    </ul>
    <a href="/serviceinsight/"><h3>All ServiceInsight articles</h3></a><br/>
    <div style="clear: both"></div>
  </div>
  <div class="productcolumn block">
    <p></p>
    <h4>Gather all the data and monitor multiple endpoints from a single location</h4>
    <p></p>
    <ul>
      <li><a href="/servicecontrol/servicecontrol-in-practice.md">Optimizing for use in different environments</a></li>
      <li><a href="/servicecontrol/installation.md">Installing ServiceControl</a></li>
    </ul>
    <a href="/servicecontrol/"><h3>All ServiceControl articles</h3></a><br/>
    <div style="clear: both"></div>
  </div>
  <div class="productcolumn last block">
    <p></p>
    <h4>Real-time monitoring that is custom tailored to fit a distributed systems</h4>
    <p></p>
    <ul>
      <li><a href="/servicepulse/intro-endpoints-heartbeats.md">Monitoring Endpoints</a></li>
      <li><a href="/servicepulse/intro-failed-messages.md">Handling Failed Messages</a></li>
    </ul>
    <a href="/servicepulse/"><h3>All ServicePulse articles</h3></a><br/>
    <div style="clear: both"></div>
  </div>
</div>
</div>
<div class="row">
<div class="productcolumn">
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServiceInsight/releases"><img src="/home/release-notes_v1.svg" height="16"/>Release notes</a></span>
  </div>
</div>
<div class="productcolumn">
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServiceControl/releases"><img src="/home/release-notes_v1.svg" height="16"/>Release notes</a></span>
  </div>
</div>
<div class="productcolumn last">
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServicePulse/releases"><img src="/home/release-notes_v1.svg" height="16"/>Release notes</a></span>
  </div>
</div>
</div>
