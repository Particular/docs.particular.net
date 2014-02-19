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
    width: 20%;
	min-width: 140px;
    float: left;
    text-align: center;
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 30px;
    margin-top: 30px;
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
  <a href="http://particular.net/NServiceBus"><img src="/images/home/NSB.png" style="max-width: 43%" /></a>
  <div class="small button">
    <a class="blue" href="/nservicebus">Documentation topics</a>
  </div>
  <div style="clear: both"></div>

</div>
<div class="block middle">
  <div class="ic">              
    <a href="/nservicebus/#getting-started" class="img">
      <img src="/images/home/gettingStarted.png" /><img src="/images/home/gettingStartedHover.png" class="hover"/><br/>
      Getting Started &rarr;
    </a>
  </div>
  <div class="ic">              
    <a href="http://particular.net/Videos-and-Presentations" class="img">
      <img src="/images/home/IntroVideos.png" /><img src="/images/home/IntroVideosHover.png" class="hover"/><br/>
      Intro Videos &rarr;
    </a>
  </div>
  <div class="ic">              
    <a href="http://particular.net/HandsOnLabs" class="img">
      <img src="/images/home/HOL.png" /><img src="/images/home/holHover.png" class="hover"/><br/>
      Hands-On Labs &rarr;
    </a>
  </div>
  <div class="ic">              
    <a href="/nservicebus/" class="img rarr">
      <img src="/images/home/API.png" /><img src="/images/home/apiHover.png" class="hover"/><br/>
      API &rarr;
    </a>
  </div>
  <div class="ic">              
    <a href="/nservicebus/" class="img rarr">
      <img src="/images/home/Samples.png" /><img src="/images/home/SamplesHover.png" class="hover"/><br/>
      Samples &rarr;
    </a>
  </div>
  <div style="clear: both"></div>
</div>
<div class="block black">
  <span class="blue"><a href="https://github.com/Particular/NServiceBus/releases"><img src="/images/home/releaseNotes.png" /> Release notes</a></span><span class="blue"><a href="http://particular.net/downloads"><img src="/images/home/download.png" /> Downloads</a></span>
</div>
<div class="block middle">            
  <div class="left2">              
    <h2>External training resources </h2>
    <a href="http://pluralsight.com/training/Courses/TableOfContents/nservicebus" class="img">
      <img src="/images/home/videosSmall.png" />
      <img src="/images/home/videosSmallHover.png" class="hover"/>
      <h3>Pluralsight Introduction to NServiceBus &rarr;</h3>
    </a>
    <p>6hrs with Andreas Öhlund, Lead developer of NServiceBus</p>    
    <div style="clear: both"></div>
    <a href="http://www.packtpub.com/build-distributed-software-systems-using-dot-net-enterprise-service-bus/book" class="img">
      <img src="/images/home/book.png" />
      <img src="/images/home/bookHover.png" class="hover"/>
      <h3>Learn NServiceBus &rarr;</h3>
    </a>
    <p>Book by David Boike. Register and get the first 3 chapters free</p>
    <div style="clear: both"></div>              
  </div>
  <div class="right1">
    <h2>Community help</h2>
    <a href="http://stackoverflow.com/questions/tagged/nservicebus" class="img">
      <img src="/images/home/stackoverflowBig.png" />
      <img src="/images/home/stackoverflowBigHover.png" class="hover"/>
      <h3>StackOverflow &rarr;</h3>
    </a>              
    <div style="clear: both"></div>
    <a href="https://groups.google.com/forum/#!forum/particularsoftware" class="img">
      <img src="/images/home/discussion.png" />
      <img src="/images/home/discussionHover.png" class="hover"/>
      <h3>Discussion group &rarr;</h3>
    </a>
    <div style="clear: both"></div>              
  </div>
  <div style="clear: both"></div>
</div>
<div class="productcolumn header">
  <div class="block top">
    <a href="http://particular.net/ServiceMatrix">
      <img src="/images/home/SM.png" />
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn header">
  <div class="block top">
    <a href="http://particular.net/ServiceInsight">
      <img src="/images/home/SI.png" />
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn header last">
  <div class="block top">
    <a href="http://particular.net/ServicePulse">
      <img src="/images/home/SP.png" />
    </a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumnc">            
  <div class="productcolumn block">
    <p>Get the logical view of your system from top-to-bottom knowing that your design is always in sync with your code.</p>
    <a href="/ServiceMatrix"><h3>Topics &rarr;</h3></a>
   <div style="clear: both"></div>
  </div>
  <div class="productcolumn block">
    <p>ServiceInsight gives you visibility across queues, processes, and
machines showing messages whose processing has failed (and for what reason) as well as their relation to other messages.</p>
    <a href="/ServiceInsight"><h3>Topics &rarr;</h3></a>
    <div style="clear: both"></div>
  </div>
  <div class="productcolumn last block">
    <p>Keep track of your system's endpoints health, monitor for any processing errors, send failed messages for reprocessing and make sure your specific environment's needs are met - All in one consolidated dashboard.</p>
    <a href="/ServicePulse"><h3>Topics &rarr;</h3></a>
    <div style="clear: both"></div>
  </div>
</div>
<div class="productcolumn">
  <div class="block">
    <h4>Community help</h4>
    <a href="http://stackoverflow.com/questions/tagged/ServiceMatrix" class="img">
      <img src="/images/home/stackoverflowSmall.png" />
      <img src="/images/home/stackoverflowSmallHover.png" class="hover" />
      <h5>StackOverflow &rarr;</h5>
    </a>
  </div>
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServiceMatrix/releases"><img src="/images/home/releaseNotes.png" /> Release notes</a></span><span class="blue"><a href="http://particular.net/downloads"><img src="/images/home/download.png" /> Downloads</a></span>
  </div>
</div>
<div class="productcolumn">
  <div class="block">
    <h4>Community help</h4>
    <a href="http://stackoverflow.com/questions/tagged/ServiceInsight" class="img">
      <img src="/images/home/stackoverflowSmall.png" />
      <img src="/images/home/stackoverflowSmallHover.png" class="hover" />
      <h5>StackOverflow &rarr;</h5>
    </a>
  </div>
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServiceInsight/releases"><img src="/images/home/releaseNotes.png" /> Release notes</a></span><span class="blue"><a href="http://particular.net/downloads"><img src="/images/home/download.png" /> Downloads</a></span>
  </div>
</div>
<div class="productcolumn last">
  <div class="block">
    <h4>Community help</h4>
    <a href="http://stackoverflow.com/questions/tagged/ServicePulse" class="img">
      <img src="/images/home/stackoverflowSmall.png" />
      <img src="/images/home/stackoverflowSmallHover.png" class="hover" />
      <h5>StackOverflow &rarr;</h5>
    </a>
  </div>
  <div class="block black">
    <span class="blue"><a href="https://github.com/Particular/ServicePulse/releases"><img src="/images/home/releaseNotes.png" /> Release notes</a></span><span class="blue"><a href="http://particular.net/downloads"><img src="/images/home/download.png" /> Downloads</a></span>
  </div>
</div>
<div style="clear: both; padding-top: 35px"></div>
