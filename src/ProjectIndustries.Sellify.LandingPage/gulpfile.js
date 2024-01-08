const gulp = require('gulp'),
  less = require('gulp-less'),
  nano = require('gulp-cssnano'),
  sourcemaps = require('gulp-sourcemaps'),
  concat = require('gulp-concat'),
  gulpIf = require('gulp-if'),
  autoprefixer = require('gulp-autoprefixer'),
  sync = require('browser-sync').create(),
  uglify = require('gulp-uglify'),
  imagemin = require('gulp-imagemin'),
  htmlExtend = require('gulp-html-extend');

const isDevelopmentMode = process.env.TARGET_ENV !== "prod";

gulp.task('css:own', function () {
  return gulp.src('src/css/main.less')
    .pipe(gulpIf(isDevelopmentMode, sourcemaps.init()))
    .pipe(less())
    .on('error', function (err) {
      console.log(err);
      this.emit('end');
    })
    .pipe(autoprefixer('last 2 versions'))
    .pipe(nano({zindex: false}))
    .pipe(gulpIf(isDevelopmentMode, sourcemaps.write()))
    .pipe(gulp.dest('dist/css'))
    .pipe(sync.stream());
});

gulp.task('css:vendor', function () {
  return gulp.src([
    'node_modules/bootstrap-reboot/dist/reboot.min.css',
    'node_modules/bootstrap-4-grid/css/grid.min.css',
    'node_modules/aos/dist/aos.css',
  ])
    .pipe(concat('vendor.css'))
    .pipe(gulp.dest('dist/css'));
});

gulp.task('images', function () {
  return gulp.src('src/images/**/*.{png,jpg,gif,jpeg,svg}')
    .pipe(gulpIf(!isDevelopmentMode, imagemin([
      imagemin.gifsicle({interlaced: true}),
      imagemin.jpegtran({progressive: true}),
      imagemin.optipng({optimizationLevel: 5}),
      imagemin.svgo({
        plugins: [
          {removeViewBox: true},
          {cleanupIDs: false}
        ]
      })
    ])))
    .pipe(gulp.dest('dist/images'));
});


gulp.task('fonts', function () {
  return gulp.src([
    'src/fonts/**/*.*'
  ])
    .pipe(gulp.dest('dist/fonts'));
});

gulp.task('js:own', function () {
  return gulp.src('src/js/*.js')
    .pipe(gulpIf(isDevelopmentMode, sourcemaps.init()))
    .pipe(concat('main.js'))
    .pipe(gulpIf(!isDevelopmentMode, uglify()))
    .pipe(gulpIf(isDevelopmentMode, sourcemaps.write()))
    .pipe(gulp.dest('dist/js'));
});

gulp.task('js:vendor', function () {
  return gulp.src([
    'node_modules/slideout/dist/slideout.js',
    'node_modules/aos/dist/aos.js',

    // this one required if we want to support IE9
    // 'node_modules/smooth-scroll/dist/smooth-scroll.polyphills.min.js',
    'node_modules/smooth-scroll/dist/smooth-scroll.min.js',

  ])
    .pipe(concat('vendor.js'))
    .pipe(gulpIf(!isDevelopmentMode, uglify()))
    .pipe(gulp.dest('dist/js'));
});

gulp.task('html', function () {
  return gulp.src('src/*.html')
    .pipe(htmlExtend())
    .pipe(gulp.dest('dist'));
});

gulp.task('css', ['css:own', 'css:vendor']);
gulp.task('js', ['js:own', 'js:vendor']);

gulp.task('watch', ['build'], function () {
  sync.init({
    server: 'dist'
  });
  gulp.watch('src/css/**/*.less', ['css:bundle']);
  gulp.watch('dist/css/**/*.css').on('change', sync.reload);

  gulp.watch('src/images/**/*.{png,jpg,gif,jpeg,svg}', ['images']);
  // gulp.watch('dist/images/**/*.{png,jpg,gif,jpeg,svg}', ['images']).on('change', sync.reload);

  gulp.watch('src/js/*.js', ['js:bundle']);
  gulp.watch('dist/js/*.js').on('change', sync.reload);

  gulp.watch('src/**/*.html', ['html']);
  gulp.watch('dist/*.html').on('change', sync.reload);
});

gulp.task('css:bundle', ["css:vendor", "css:own"], function () {
  // if (isDevelopmentMode) {
  //   this.emit("end");
  //   return;
  // }

  return gulp.src([
    'dist/css/vendor.css',
    'dist/css/main.css'
  ])
    .pipe(concat("bundle.min.css"))
    .pipe(gulp.dest("dist/css"));
});
gulp.task('js:bundle', ["js:vendor", "js:own"], function () {
  // if (isDevelopmentMode) {
  //   this.emit("end");
  //   return;
  // }

  return gulp.src([
    'dist/js/vendor.js',
    'dist/js/main.js'
  ])
    .pipe(concat("bundle.min.js"))
    .pipe(gulp.dest("dist/js"));
});

gulp.task('build', ['css', 'css:bundle', 'js:bundle', 'images', 'fonts', 'html']);
gulp.task('default', ['build', 'watch']);
