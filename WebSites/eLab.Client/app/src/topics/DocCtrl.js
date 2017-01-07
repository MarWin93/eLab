angular.module('eLabApp').controller('DocCtrl', function ($scope, $rootScope, PDFViewerService) {

    $scope.instance = PDFViewerService.Instance("pdfViewer");
    $scope.pdfURL = '/app/src/firstPage.pdf';

    $scope.signalRing = function (page) {
        $scope.newMessage({
            operation: 'gotoPage',
            page: page
        });
    };

    $scope.nextPage = function () {
        $scope.signalRing($scope.currentPage+1);
    };

    $scope.prevPage = function () {
        $scope.signalRing($scope.currentPage-1);
    };

    $scope.gotoPage = function (page) {
        var total = $scope.totalPages;
        page = parseInt(page);

        if (page > total) {
            $scope.newPage = total;
            page = total;
        }
        else if (!page || page < 1) {
            $scope.newPage = 1;
            page = 1;
        }

        $scope.signalRing(page);
    };

    $scope.pageLoaded = function (curPage, totalPages) {
        $scope.currentPage = curPage;
        $scope.totalPages = totalPages;
    };

    $scope.loadProgress = function (loaded, total, state) {
        //console.log('loaded =', loaded, 'total =', total, 'state =', state);
    };

    $rootScope.$on('gotoPage', function (event, page) {
        $scope.instance.gotoPage(page);
    });

    $rootScope.$on('changePDF', function (event, url) {
        $scope.pdfURL = url;
        console.log($scope.pdfURL);
        $scope.instance.gotoPage(1);
    });
});