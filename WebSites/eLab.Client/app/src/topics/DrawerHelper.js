 angular.module('eLabApp').service('drawerHelper', function () {
     var element = document.getElementById('canvas');
     var ctx = element.getContext("2d");
     var tool = 'pencil';
     var lineWidth = 3;
     var highlighterBegin = false;
     var opacity = 1;
     var color = '0,0,0';

     self.fromSignalR = function (parameters) {
         self.setLineWidth(parameters.lineWidth);

         var X = parameters.x * element.width;
         var Y = parameters.y * element.height;
         var X2 = parameters.x2 * element.width;
         var Y2 = parameters.y2 * element.height;

         switch (parameters.operation) {
             case 'beginPath':
                 ctx.moveTo(X, Y);
                 ctx.beginPath();
                 break;
             case 'draw':
                 self.setTool(parameters.tool);
                 self.drawLine(X, Y, parameters.color);
                 break;
             case 'highlight':
                 self.setTool(parameters.tool);
                 ctx.moveTo(X, Y);
                 self.drawLine(X2, Y2, parameters.color);
                 ctx.closePath();
                 break;
             case 'endPath':
                 ctx.closePath();
                 break;
         }
     };

     self.drawLine = function draw(currentX, currentY, color) {
         ctx.strokeStyle = 'rgba(' + color + ', ' + opacity + ')';
         console.log(ctx.strokeStyle);
         ctx.lineTo(currentX, currentY);
         ctx.stroke();
     };

     self.setTool = function (toolName) {
         if (toolName == 'highlighter') {
             tool = toolName;
             self.setLineWidth(30);
             self.setOpacity(0.3);
         }
         else {
             tool = 'pencil';
             self.setLineWidth(3);
             self.setOpacity(1);
         }
     };

     self.getTool = function () {
         return tool;
     };

     self.setLineWidth = function (width) {
         lineWidth = width > 0 && width <= 30 ? width : 3;
         ctx.lineWidth = lineWidth;
     };

     self.getLineWidth = function () {
         return lineWidth;
     };

     self.setColor = function (clr) {
         color = clr;
     };

     self.getColor = function () {
         return color;
     };

     self.setOpacity = function (opc) {
         opacity = opc > 0 && opc <= 1 ? opc : 1;
     };

     // canvas reset
     self.reset = function () {
         element.width = element.width;
     };

     return self;
 });
