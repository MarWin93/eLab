angular.module('eLabApp').service('drawerHelper', function ($rootScope) {
     var tool = 'pencil';
     var lineWidth = 3;
     var highlighterBegin = false;
     var opacity = 1;
     var color = '0,0,0';

     self.fromSignalR = function (parameters) {
         if (!self.element) {
             self.element = document.querySelector('#pdfViewer canvas');
             self.canvas = angular.element(self.element);
             self.ctx = self.element.getContext("2d");
         }

         self.setLineWidth(parameters.lineWidth);

         var xScale = self.element.width;// / self.element.offsetWidth;
         var yScale = self.element.height;// / self.element.offsetHeight;

         var X = parameters.x * xScale;
         var Y = parameters.y * yScale;
         var X2 = parameters.x2 * xScale;
         var Y2 = parameters.y2 * yScale;
         switch (parameters.operation) {
             case 'draw':
             case 'highlight':
                 self.setTool(parameters.tool);
                 self.drawLine(X, Y, X2, Y2, parameters.color);
                 break;
             case 'gotoPage':           // move it to PDFHelper
                 $rootScope.$broadcast('gotoPage', parameters.page);
         }
     };

     self.drawLine = function draw(X, Y, X2, Y2, color) {

         self.ctx.beginPath();
         self.ctx.moveTo(X, Y);
         self.ctx.strokeStyle = 'rgba(' + color + ', ' + opacity + ')';
         self.ctx.lineTo(X2, Y2);
         self.ctx.stroke();

         //self.ctx.closePath();
     };

     self.setTool = function (toolName) {
         if (toolName == 'highlighter') {
             tool = toolName;
             self.setLineWidth(30);
             self.setOpacity(0.3);

             self.canvas.removeClass('paint-with-pencil color-0-0-0');
             self.canvas.addClass('paint-with-brush');
         }
         else {
             tool = 'pencil';
             self.setLineWidth(3);
             self.setOpacity(1);

             self.canvas.addClass('paint-with-pencil');
             self.canvas.removeClass('paint-with-brush');
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
         console.log(color, '4d8as4d8sa4d8sa4d9as88dsaaadsadas>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>');
         self.canvas.removeClass(function (index, css) {
             return (css.match(/(^|\s)color-\S+/g) || []).join(' ');
         });
         self.canvas.addClass('color-' + color.replace(/,/g, "-"));
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
