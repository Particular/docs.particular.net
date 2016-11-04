<Query Kind="Program" />

void Main()
{
	var gen = new DiagramGenerator();

	gen.BoxStyle("normal", "fill:rgb(238,238,238);stroke-width:2;stroke:rgb(0,0,0)");
	var external = gen.BoxStyle("external", "fill:rgb(238,238,238);stroke-width:2;stroke:rgb(89,89,89);stroke-dasharray: 5,3");
	var outgoing = gen.BoxStyle("outgoing", "fill:rgb(217,234,211);stroke-width:2;stroke:rgb(0,0,0)");
	var incoming = gen.BoxStyle("incoming", "fill:rgb(164,194,244);stroke-width:2;stroke:rgb(0,0,0)");
	
	gen.EdgeStyle("normal", "stroke: black;stroke-width:2");
	var optional = gen.EdgeStyle("optional", "stroke: black;stroke-width:2;stroke-dasharray:5,2,2");
	var fork = gen.EdgeStyle("fork", "stroke: black;stroke-width:2;stroke-dasharray: 5,2");

	var userCode = gen.Box(1, 2, "User Code", external);
	
	var outgoingPublish = gen.Box(2, 1.2m, "Outgoing Publish", outgoing);
	var outgoingSend = gen.Box(2, 2, "Outgoing Send", outgoing);
	var outgoingReply = gen.Box(2, 2.8m, "Outgoing Reply", outgoing);
	
	var outgoingLogical = gen.Box(3, 2, "Outgoing Logical Message", outgoing);
	var outgoingPhysical = gen.Box(4, 2, "Outgoing Physical Message", outgoing);
	var routing = gen.Box(4, 3, "Routing", outgoing);
	var dispatch = gen.Box(5.5m, 3, "Dispatch");
	
	var invokeHandler = gen.Box(2, 5, "Invoke Handler", incoming);
	var incomingLogical = gen.Box(3, 5, "Incoming Logical Message", incoming);
	var incomingPhysical = gen.Box(4, 5, "Incoming Physical Message", incoming);
	
	var transportReceive = gen.Box(5.5m, 5, "Transport Receive");
	
	var transport = gen.Box(6.5m, 4, "Transport", external);
	
	var batchDispath = gen.Box(5.5m, 4, "Batch Dispatch");
	
	var audit = gen.Box(4.5m, 4, "Audit");
	var forwarding = gen.Box(3.5m, 4, "Forwarding");

//	gen.Box(5.5m, 1, "Subscribe");
//	gen.Box(6.5m, 1m, "Unsubscribe");

	gen.Edges(f => new[] {
			f.RhsToLhs(userCode, outgoingSend, optional), 
			f.RhsToLhs(userCode, outgoingPublish, optional), 
			f.RhsToLhs(userCode, outgoingReply, optional),
			f.RhsToLhs(outgoingSend, outgoingLogical), 
			f.RhsToLhs(outgoingPublish, outgoingLogical), 
			f.RhsToLhs(outgoingReply, outgoingLogical),
			f.RhsToLhs(outgoingLogical, outgoingPhysical),
			
			f.BottomToTop(outgoingPhysical, routing),
			
			f.RhsToLhs(routing, dispatch),
			f.RhsToTop(dispatch, transport),
			
			f.BottomToRhs(transport, transportReceive),
			
			f.LhsToRhs(transportReceive, incomingPhysical),
			f.LhsToRhs(incomingPhysical, incomingLogical),
			f.LhsToRhs(incomingLogical, invokeHandler),
			f.LhsToBottom(invokeHandler, userCode),
			
			f.TopToBottom(transportReceive, batchDispath, fork),
			f.TopToBottom(batchDispath, dispatch), 
			
			f.TopToBottom(incomingPhysical, audit, fork),
			f.TopToBottom(audit, routing),
			f.TopToBottom(incomingPhysical, forwarding, fork), 
			f.TopToBottom(forwarding, routing)
		});
		
	var xml = gen.ToXml();
		
	new XDocument(xml).Save(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "context-map.svg")); 
		
	Util.RawHtml(xml).Dump();
}

#region Support

class DiagramGenerator
{
	private readonly IList<Box> boxes = new List<Box>();
	private readonly List<Edge> edges = new List<Edge>();

	public const int boxHeight = 120;
	public const int boxWidth = 190;
	public const int gap = 40;
	public const int arrowLength = 10;
	
	public XNamespace ns = XNamespace.Get("http://www.w3.org/2000/svg");

	private IDictionary<string, string> boxStyles = new Dictionary<string, string>
	{
		["normal"] = "fill:rgb(255,255,255);stroke-width:2;stroke:rgb(0,0,0)"
	};

	private IDictionary<string, string> edgeStyles = new Dictionary<string, string>
	{
		["normal"] = "stroke-width:1;stroke:black"
	};

	public string BoxStyle(string name, string style)
	{
		boxStyles[name] = style;
		return name;
	}
	
	public string EdgeStyle(string name, string style)
	{
		edgeStyles[name] = style;
		return name;
	}

	public void Edges(Func<EdgeFactory, IEnumerable<Edge>> configureEdges)
	{
		var edgeFactory = new EdgeFactory();
		edges.AddRange(configureEdges(edgeFactory));
	}

	public Box Box(decimal x, decimal y, string text, string style = "normal")
	{
		var box = new Box { X = x, Y = y, Text = text, Style = style };
		boxes.Add(box);
		return box;
	}


	public XElement ToXml()
	{
		var width = boxes.Max(x => Right(x) + gap);
		var height = boxes.Max(x => Bottom(x) + gap);
		
		var svgAttributes = new object[] {
				new XAttribute("width", width),
				new XAttribute("height", height),
				new XElement(ns + "defs",
					GetDefs()
				)
			};

		return new XElement(ns + "svg",
			svgAttributes
				.Concat(GetBoxes())
				.Concat(GetEdges())
		);
	}

	private IEnumerable<object> GetDefs()
	{
		yield return new XElement(ns + "marker",
			new XAttribute("id", "arrow"),
			new XAttribute("markerWidth", "10"),
			new XAttribute("markerHeight", "10"),
			new XAttribute("refX", "4"),
			new XAttribute("refY", "3"),
			new XAttribute("orient", "auto"),
			new XAttribute("markerUnits", "strokeWidth"),
			new XAttribute("stroke-dasharray", "none"),
			new XAttribute("stroke-width", "1"),
			new XElement(ns + "path",
				new XAttribute("d", "M0,0 L0,6 L9,3 L0,0"),
				new XAttribute("fill", "#000")
			)
		);
	}

	private IEnumerable<object> GetEdges()
	{
		foreach (var edge in edges)
		{
			yield return new XElement(ns + "path",
				new XAttribute("d", edge.EdgeStatement),
				new XAttribute("style", edgeStyles[edge.Style]),
				new XAttribute("marker-end", "url(#arrow)"), 
				new XAttribute("fill", "none")
			);
		}

	}

	private IEnumerable<object> GetBoxes()
	{
		var boxStylesLookup = boxStyles.ToLookup(kvp => kvp.Key, kvp => kvp.Value);

		foreach (var box in boxes)
		{
			yield return new XElement(ns + "rect",
				new XAttribute("x", Left(box)),
				new XAttribute("y", Top(box)),
				new XAttribute("width", boxWidth),
				new XAttribute("height", boxHeight),
				new XAttribute("style", boxStylesLookup[box.Style].FirstOrDefault())
			);

			var words = box.Text.Split(' ');

			var textOffset = (words.Length - 1) * -0.45m;

			var tspans = words.Select((w, i) => (object)new XElement(ns + "tspan",
				new XAttribute("x", Left(box) + (boxWidth / 2)),
				new XAttribute("dy", i == 0 ? $"{textOffset}em" : "1.2em"),
				w));

			if (words.Length == 1)
			{
				tspans = words;
			}

			yield return new XElement(ns + "text",
				new object[] {
					new XAttribute("x", Left(box) + (boxWidth / 2)),
					new XAttribute("y", Top(box)  + (boxHeight / 2)),
					new XAttribute("text-anchor", "middle"),
					new XAttribute("alignment-baseline", "middle"),
					new XAttribute("font-size", "20pt"),
					new XAttribute("font-family", "Arial")
				}.Concat(tspans)

			);
		}
	}
	
	static int Left(Box box)
	{
		return (int)((box.X - 1) * (2 * gap + boxWidth) + gap);
	}
	
	static int Right(Box box)
	{
		return Left(box) + boxWidth;		
	}

	static int Top(Box box)
	{
		return (int)((box.Y - 1) * (2 * gap + boxHeight) + gap);
	}
	
	static int Bottom(Box box)
	{
		return Top(box) + boxHeight;
	}

	public class EdgeFactory
	{
		public Edge RhsToLhs(Box start, Box end, string style = "normal")
		{
			var startX = Right(start);
			var startY = Top(start) + boxHeight / 2;
			
			var endX = Left(end) - arrowLength;
			var endY = Top(end) + boxHeight / 2;
			
			var halfX = (endX - startX) / 2;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} h {halfX} v {endY - startY} h {halfX}", 
				Style = style
			};
		}

		public Edge LhsToRhs(Box start, Box end, string style = "normal")
		{
			var startX = Left(start);
			var startY = Top(start) + boxHeight / 2;
			
			var endX = Right(end) + arrowLength;
			var endY = Top(end) + boxHeight / 2;
			
			var halfX = (endX - startX) / 2;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} h {halfX} v {endY - startY} h {halfX}", 
				Style = style
			};
		}

		public Edge BottomToTop(Box start, Box end, string style = "normal")
		{
			var startX = Left(start) + boxWidth / 2;
			var startY = Bottom(start);
			
			var endX = Left(end) + boxWidth / 2;
			var endY = Top(end) - arrowLength;
			
			var halfY = (endY - startY) / 2;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} v {halfY} h {endX - startX} v {halfY}", 
				Style = style
			};
		}

		public Edge TopToBottom(Box start, Box end, string style = "normal")
		{
			var startX = Left(start) + boxWidth / 2;
			var startY = Top(start);
			
			var endX = Left(end) + boxWidth / 2;
			var endY = Bottom(end) + arrowLength;
			
			var halfY = (endY - startY) / 2;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} v {halfY} h {endX - startX} v {halfY}", 
				Style = style
			};
		}

		public Edge LhsToBottom(Box start, Box end, string style = "normal")
		{
			var startX = Left(start);
			var startY = Top(start) + boxHeight / 2;
			
			var endX = Left(end) + boxWidth / 2;
			var endY = Bottom(end) + arrowLength;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} h {endX - startX} v {endY - startY}", 
				Style = style
			};
		}

		public Edge RhsToTop(Box start, Box end, string style = "normal")
		{
			var startX = Right(start);
			var startY = Top(start) + boxHeight / 2;
			
			var endX = Left(end) + boxWidth / 2;
			var endY = Top(end) - arrowLength;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} h {endX - startX} v {endY - startY}", 
				Style = style
			};
		}

		public Edge BottomToRhs(Box start, Box end, string style = "normal")
		{
			var startX = Left(start) + boxWidth / 2;
			var startY = Bottom(start);
			
			var endX = Right(end) + arrowLength;
			var endY = Top(end) + boxHeight / 2;

			return new Edge
			{
				EdgeStatement = $"M {startX} {startY} v {endY - startY} h {endX - startX}", 
				Style = style
			};
		}

	}
}

class Box
{
	public decimal X { get; set; }
	public decimal Y { get; set; }
	public string Text { get; set; }
	public string Style { get; set; }
}

class Edge
{
	public string EdgeStatement { get; set; }
	public string Style { get; set; }
}

#endregion