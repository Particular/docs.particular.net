### Images

Images can be added using the following markdown syntax

```markdown
![Alt text](/path/to/img.jpg "Optional title")
```

With the minimal syntax being

```markdown
![](/path/to/img.jpg)
```

#### Image sizing

Image size can be controlled by adding the text `width=x` to the end of the title

For example

```markdown
![Alt text](/path/to/img.jpg "Optional title width=x")
```

With the minimal syntax being

```markdown
![](/path/to/img.jpg "width=x")
```

This will result in the image being re-sized with the following parameters

```text
width="x" height="auto"
```

It will also wrap the image in a clickable lightbox so the full image can be accessed.

#### Maintaining images

Whenever possible, use Mermaid to create images, so that the source of the image is part of the document in which it appears.

If you create an image using another tool, always keep the source so that the image can be updated later.

#### Mermaid

The support for [Mermaid](https://mermaid.js.org) is provided as an extension to [Markdig](https://github.com/lunet-io/markdig). Markdig converts the diagram definition from .md to HTML, and then the Mermaid JavaScript library converts the definition to SVG format on the fly.

Diagram images are generated using the  using a pseudocode syntax like this:

```markdown
    ```mermaid
    _mermaid_diagram_definition_
    ```
```

For example:

```markdown
    ```mermaid
    graph TB
    A[ExchangeA] --> B[ExchangeB]
    A --> D[ExchangeD]
    B --> C[ExchangeC]
    B --> Q1[Queue1]
    D --> Q2[Queue2]
    ```
```

The diagrams can be created and verified using the [online editor](http://knsv.github.io/mermaid/live_editor/).

##### Messaging Graph Style

Diagrams that represent messages and events being passed between endpoint should follow some basic style rules.

Endpoints should be represented as nodes with rounded corners. Messages should be represented as nodes. To show an endpoint sending a message to another endpoint use two edges. The first edge goes from the sender to the message being sent. The second edge goes from the message to the receiver. Like this:

```markdown
    ```mermaid
    graph LR
    a(EndpointA)
    b(EndpointB)
    a-->SomeCommand
    SomeCommand-->b
    ```
```

Showing an endpoint publishing an event is similar but should use a dotted edge. Events can be delivered to multiple recipients. Use a separate edge for each one. Like this:

```markdown
    ```mermaid
    graph LR
    a(EndpointA)
    b(EndpointB)
    c(EndpointC)
    a-.->AnEvent
    AnEvent-->b
    AnEvent-->c
    ```
```

There are two css classes (`event` and `message`) that should be applied to message nodes in these diagrams. To apply these, use the `class` keyword in mermaid:

```markdown
    ```mermaid
    graph LR

    Endpoint1-->SomeCommand
    Command-->Endpoint2
    Endpoint2-.->AnEvent
    AnEvent-.->Endpoint3
    Endpoint3 -.->AnotherEvent
    AnotherEvent -.->Endpoint1
    AnotherEvent -.->Endpoint4

    class SomeCommand message;
    class AnEvent,AnotherEvent event;
    ```
```

#### Miro

Another option is [Miro](https://miro.com/). Miro allows the creation of diagrams on a whiteboard. A board can be saved as a board backup file (.rtb) and can be used to create images. Within Miro, we create draw.io diagrams, as these can be exported as PNG images.

To create an image:

- Double-click the draw.io diagram to open the diagram in the draw.io editor
- From the "File" menu:
    - Click "Export as"
    - Click "PNG.."
    - Set Zoom to 200% to cater for scaled-up high resolution screens.
    - Do NOT select "Transparent Background", as this makes the visibility of elements in the image dependent on the user's theme (e.g. light or dark).
- Choose the file location and click "Export"

To allow images to be updated later, they must be committed to this repository along with the board backup file(s) use to create them.