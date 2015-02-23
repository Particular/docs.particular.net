<style>
  a.img img.hover{display:none;}
  a.img:hover img{display:none;}
  a.img:hover img.hover{display:inline;}
	
  .small.button{
	line-height: 45px;
	font-size: 16px;
	padding-left: 15px;
	font-family: 'Lato',Bold;
	display: inline-block;
  }
  .small.button a{
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
  .small.button a:hover{
	background-color: #03AFF8;
  }
  .block.black a, .block.middle a, .productcolumn a {
    color: inherit;
	white-space: nowrap;
  }
  .block{
    width: 100%;
    background-color: rgb(233,240,242);
    padding: 21px;
    margin-bottom: 2px;
    font-family: 'Lato';
  }

  .block.top img, .productcolumn img {
    float: left;
  }
  .block.black img{
    float: inherit;
  }
  .block.top .button{
    float: right;
    width: 225px;
    font-size: 15px;
  }
  .block.middle .ic{
    min-width: 25%;
    float: left;
    text-align: center;
    font-size: 18px;
    font-weight: bold;
    line-height: 50px;
    color: rgb(0,114,156);
  }      
  .block.black{
    margin-top: -2px;
    margin-bottom: 0px;
    width: 100%;
    clear: both;
    background-color: rgb(26,26,26);
    font-size: 16px;
    font-weight: bold;
    padding-top: 13px;
    padding-bottom: 13px;
    line-height: 30px;
  }
  span.blue{
    color: rgb(0,163,196);
    padding-right: 30px;
    display: inline-block;
  }
  .block.black span img{
    padding-left: 0px;
    padding-right: 5px;
    margin-top: -3px;
  }
  .productcolumn .black{
    font-size: 14px;
  }
  .block .left2 {
    width: 60%;
    float: left;
    border-right: 2px solid rgb(218,222,222);
    min-width: 460px;
  }
  .block .right1 {
    float: left;
    padding-left: 20px;
  }
  .block .right1 h3{
    padding-top: 7px;
  }
  .block h2{
    clear: both;
    font-size: 20px !important;
    font-family: 'Dosis', Semibold;
    padding-bottom: 20px;
    margin-bottom: 0px;
    margin-top: 0px;
  }
  .block h3{
    font-weight: bold;
    font-size: 17px;
    margin-top: 0px;
    margin-bottom: 0px;
    color: rgb(0,114,156);
  }
  .block h4{
    font-size: 16px !important;
    font-family: 'Dosis', bold;
    font-weight: bold;
    margin-top: 0px;
  }
  .block h5{        
    color: rgb(0,114,156);
    font-size: 14px;
    font-weight: bold;
    padding-left: 28px;
    padding-top: 5px;
  }
  .block p{
    font-size: 14px;
    color: rgb(77,77,77);
  }
  .block .right1 img, .block .left2 img {
    float: left;
    margin: 0px 13px 23px 0px;
  }
  .productcolumn{
    width: 32%;
    margin-right: 2%;
    float: left;        
  }
  .productcolumn.header{
    margin-top: 35px;        
  }
  .productcolumn.last{
    margin-right: 0px;
  }
  .productcolumnc{
    overflow: hidden;
    clear: both;
  }
  .productcolumnc .productcolumn{
    padding-bottom: 1000px;
    margin-bottom: -1000px;
  }
  
  .productcolumnc ul {
    list-style: none;
    margin-left: 0px;
    padding-left: 0px;
  }
  .productcolumnc li {
    color: rgb(0,114,156) !important;
    font-size: 14px;
    font-weight: bold;
    padding-bottom: 7px;
    padding-left: 12px;
    text-indent: -12px;
  }
  .productcolumnc li:before{
    content: "• ";
    color: rgb(0,114,156);
  }
</style>

<div class="block top">
  <a href="/nservicebus"><img src="/home/nservicebus.png" style="max-width: 43%" /></a>
  <div class="small button">
    <a class="blue" href="/nservicebus/">Documentation topics</a>
  </div>
  <div style="clear: both"></div>
</div>
<div class="block middle">
  <div class="ic">
    <a href="/samples/step-by-step/" class="img">
      <img src="/home/getting-started.png" /><img src="/home/getting-started-hover.png" class="hover"/><br/>
      Getting Started
    </a>
  </div>
  <div class="ic">
    <a href="http://particular.net/Videos-and-Presentations" class="img">
      <img src="/home/intro-videos.png" /><img src="/home/intro-videos-hover.png" class="hover"/><br/>
      Intro Videos
    </a>
  </div>
  <div class="ic">
    <a href="https://groups.google.com/forum/#!forum/particularsoftware" class="img">
      <img src="/home/discussion-large.png" /><img src="/home/discussion-large-hover.png" class="hover"/><br/>
      Discussion group
    </a>
  </div>
  <div class="ic">
    <a href="/samples/" class="img rarr">
      <img src="/home/samples.png" /><img src="/home/samples-hover.png" class="hover"/><br/>
      Samples
    </a>
  </div>
  <div style="clear: both"></div>
</div>
<div class="block black">
  <span class="blue"><a href="https://github.com/Particular/NServiceBus/releases"><img src="/home/release-notes.png" /> Release notes</a></span><span class="blue"><a href="http://particular.net/downloads"><img src="/home/download.png" /> Downloads</a></span>
</div>
<div class="block middle">
<div class="left2">
    <a href="http://www.pluralsight.com/courses/table-of-contents/nservicebus" class="img">
      <img src="/home/videos-small.png" />
      <img src="/home/videos-small-hover.png" class="hover"/>
      <h3>Pluralsight Introduction to NServiceBus</h3>
    </a>
    <p>6hrs with Andreas Öhlund, Lead developer of NServiceBus</p>
    <div style="clear: both"></div>
    <a href="https://www.packtpub.com/application-development/learning-nservicebus" class="img">
      <img src="/home/book.png" />
      <img src="/home/book-hover.png" class="hover"/>
      <h3>Learn NServiceBus</h3>
    </a>
    <p>Book by David Boike. Register and get the first 3 chapters free</p>
    <div style="clear: both"></div>
  </div>
  <div class="right1">
    <a href="http://particular.net/HandsOnLabs" class="img">
      <img src="/home/hand-on-labs-small.png" />
      <img src="/home/hand-on-labs-small-hover.png" class="hover"/>
      <h3>Hands-On Labs</h3>
    </a>
    <div style="clear: both"></div>
    <a href="http://stackoverflow.com/questions/tagged/nservicebus" class="img">
      <img src="/home/stackoverflow-big.png" />
      <img src="/home/stackoverflow-big-hover.png" class="hover"/>
      <h3>StackOverflow</h3>
    </a>
    <div style="clear: both"></div>
    <a href="/platform/extensions" class="img">
      <h3>Extensions and integrations</h3>
    </a>
    <div style="clear: both"></div>
  </div>
  <div style="clear: both"></div>
</div>
<div class="productcolumn header">
  <div class="block top">
    <a href="/servicematrix/">
      <img src="/home/servicematrix.png" />
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn header">
  <div class="block top">
    <a href="/serviceinsight/">
      <img src="/home/serviceinsight.png" />
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn header last">
  <div class="block top">
    <a href="/servicepulse/">
      <img src="/home/servicepulse.png" />
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumnc">
  <div class="productcolumn block">
    <p><h4>Generate a fully functional distributed application skeleton in a matter of minutes</h4></p>
    <ul>
      <li><a href="/servicematrix/getting-started-with-servicematrix-2.0.md">Getting Started</a></li>
      <li><a href="/servicematrix/getting-started-with-nservicebus-using-servicematrix-2.0-publish-subscribe.md">Publish/Subscribe</a></li>
      <li><a href="/servicematrix/getting-started-with-nservicebus-using-servicematrix-2.0-fault-tolerance.md">Fault Tolerance</a></li>
    </ul>
    <a href="/servicematrix/"><h3>Learn more</h3></a><br/>
   <div style="clear: both"></div>
  </div>
  <div class="productcolumn block">
    <p><h4>Complete under-the-hood visualization of the your system's behavior</h4></p>
    <ul>
      <li><a href="/serviceinsight/getting-started-overview.md">Getting Started</a></li>
      <li><a href="/servicematrix/servicematrix-serviceinsight.md">Interaction with ServiceMatrix</a></li>
      <li><a href="/serviceinsight/application-invocation.md">Application invocation</a></li>
    </ul>
    <a href="/serviceinsight/"><h3>Learn more</h3></a><br/>
    <div style="clear: both"></div>
  </div>
  <div class="productcolumn last block">
    <p><h4>Real-time monitoring that is custom tailored to fit your distributed systems</h4></p>
    <ul>
      <li><a href="/servicepulse/intro-endpoints-heartbeats.md">Monitoring Endpoints</a></li>
      <li><a href="/servicepulse/intro-failed-messages.md">Handling Failed Messages</a></li>
      <li><a href="/servicepulse/intro-endpoints-custom-checks.md">Introduction to Custom Checks</a></li>
    </ul>
    <a href="/servicepulse/"><h3>Learn more</h3></a><br/>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn">
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServiceMatrix/releases"><img src="/home/release-notes.png" /> Release notes</a></span>
  </div>
</div>
<div class="productcolumn">
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServiceInsight/releases"><img src="/home/release-notes.png" /> Release notes</a></span>
  </div>
</div>
<div class="productcolumn last">
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServicePulse/releases"><img src="/home/release-notes.png" /> Release notes</a></span>
  </div>
</div>
<div style="clear: both; padding-top: 35px"></div>
