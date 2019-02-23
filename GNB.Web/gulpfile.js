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
    'datatables.net': {
        'js/*': '',
    },
    'sweetalert2': {
        'dist/*': '',
    },
    'jquery-validation': {
        'dist/*': '',
    },
    'jquery-validation-unobtrusive': {
        'dist/*': '',
    },
    'perfect-scrollbar': {
        'dist/*': 'js',
        'css/*': 'css'
    },
};

const pathLibrary = 'wwwroot/lib/thirdPartyLibrary/';

gulp.task('clean', function (cb) {
    console.log(`Clean folder ${pathLibrary}`);
    return rimraf(pathLibrary, cb);
});

gulp.task('transport', function () {
    var streams = [];

    for (var dependency in dependencies) {
        for (var itemdependency in dependencies[dependency]) {
            let source = `node_modules/${dependency}/${itemdependency}`;
            let destinationFolder = `${pathLibrary}${dependency.replace('@','')}/${dependencies[dependency][itemdependency]}`;
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