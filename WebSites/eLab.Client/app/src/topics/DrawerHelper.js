angular.module('eLabApp').service('drawerHelper', function ($rootScope) {
     var tool = 'pencil';
     var lineWidth = 3;
     var highlighterBegin = false;
     var opacity = 1;
     var color = '0,0,0';
     //console.log("Root from drawhelper:" + $rootScope.user.id);

     self.fromSignalR = function (parameters) {
         if (!self.element) {
             self.element = document.querySelector('#pdfViewer canvas');
             self.ctx = self.element.getContext("2d");
         }

         self.setLineWidth(parameters.lineWidth);

         var X = parameters.x * self.element.width;
         var Y = parameters.y * self.element.height;
         var X2 = parameters.x2 * self.element.width;
         var Y2 = parameters.y2 * self.element.height;

         switch (parameters.operation) {
             case 'beginPath':
                 self.ctx.moveTo(X, Y);
                 self.ctx.beginPath();
                 break;
             case 'draw':
                 self.setTool(parameters.tool);
                 self.drawLine(X, Y, parameters.color);
                 break;
             case 'highlight':
                 self.setTool(parameters.tool);
                 self.ctx.moveTo(X, Y);
                 self.drawLine(X2, Y2, parameters.color);
                 self.ctx.closePath();
                 break;
             case 'endPath':
                 self.ctx.closePath();
                 break;
             case 'gotoPage':           // move it to PDFHelper
                 $rootScope.$broadcast('gotoPage', parameters.page);
         }
     };

     self.drawLine = function draw(currentX, currentY, color) {
         self.ctx.strokeStyle = 'rgba(' + color + ', ' + opacity + ')';
         console.log(ctx.strokeStyle);
         self.ctx.lineTo(currentX, currentY);
         self.ctx.stroke();
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
         self.ctx.lineWidth = lineWidth;
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
