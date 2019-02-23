const gulp = require('gulp');
const rimraf = require('rimraf');
const merge = require('merge-stream');

const dependencies = {
    'jquery': {
        'dist/*': ''
    },
    'bootstrap': {
        'dist/**/*': ''
    },
    'animate.css': {
        '/animate.*': '',
    },
    'pace-js': {
        '/pace.*': '',
    },
    'jquery-slimscroll': {
        '/jquery.*': '',
    },
    'js-cookie': {
        'src/*' : '',
    },
    'select2': {
        'dist/**/*': '',
    },
    'select2-theme-bootstrap4': {
        'dist/*': ''
    },
    'bootstrap4-datetimepicker': {
        'build/**/*': '',
    },
    'moment': {
        'min/*': '',
    },
    'datatables.net': {
        'js/*': '',
    },
    'datatables.net-bs4': {
        'css/*': 'css',
        'js/*': 'js'
    },
    'datatables.net-buttons': {
        'js/*': '',
    },
    'sweetalert2': {
        'dist/*': '',
    },
    'underscore': {
        'underscore*': '',
    },
    'jstree': {
        'dist/**/*': '',
    },
    'fullcalendar': {
        'dist/**/*': '',
    },
    'switchery': {
        'standalone/*': '',
    },
    'jquery-validation': {
        'dist/*': '',
    },
    'jquery-validation-unobtrusive': {
        'dist/*': '',
    },
    'morris.js': {
        'morris.*': '',
    },
    'raphael': {
        'raphael.*': '',
    },
};

const pathLibrary = 'wwwroot/lib/thirdPartyLibrary';

gulp.task('clean', function (cb) {
    console.log(`Clean folder ${pathLibrary}`);
    return rimraf(pathLibrary, cb);
});

gulp.task('transport', function () {
    var streams = [];

    for (var dependency in dependencies) {
        for (var itemdependency in dependencies[dependency]) {
            let source = `node_modules/${dependency}/${itemdependency}`;
            let destinationFolder = `wwwroot/lib/${dependency.replace('@','')}/${dependencies[dependency][itemdependency]}`;
            console.log(`Transfer library ${dependency} from ${source} to ${destinationFolder}`);
            streams.push(gulp.src(source)
                .pipe(gulp.dest(destinationFolder)));
        }
    }

    return merge(streams);

});

gulp.task('default', gulp.series('clean', 'transport', function (done) {
    done();
}));