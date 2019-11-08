<?xml version="1.0" encoding="UTF-8"?>
<html>
    <head>
        <title>Welcome to my site!</title>
    </head>
    <body>
        <h1><%= ViewData!Message %></h1>
        <h2>This is a header.</h2>
        <p>Today is <%= Date.Now.ToString("MMM d, yyyy") %>.</p>

        <%= RenderSection(Of NavLinks)() %>
    </body>
</html>