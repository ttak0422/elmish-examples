const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const CONFIG = {
    indexHtmlTemplate: "./src/index.html",
    fsharpEntry: "./docs.fsproj",
    cssEntry: "./sass/main.sass",
    outputDir: "./deploy",
    assetsDir: "./public",
    devServerPort: 8888,
    babel: {
    }
}

const isProduction = !process.argv.find(v => v.indexOf('webpack-dev-server') !== -1);
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

const commonPlugins = [
    new HtmlWebpackPlugin({
        filename: 'index.html',
        template: resolve(CONFIG.indexHtmlTemplate)
    })
];

module.exports = {
    entry: 
        isProduction 
        ? { app: [resolve(CONFIG.fsharpEntry), resolve(CONFIG.cssEntry)] } 
        : {
            app: [resolve(CONFIG.fsharpEntry)],
            style: [resolve(CONFIG.cssEntry)]
        },
    output: {
        path: resolve(CONFIG.outputDir),
        filename: isProduction ? '[name].[hash].js' : '[name].js'
    },
    mode: isProduction ? "production" : "development",
    devtool: isProduction ? "source-map" : "eval-source-map",
    plugins: isProduction ?
        commonPlugins.concat([
            new MiniCssExtractPlugin({ filename: 'style.css' }),
            new CopyWebpackPlugin([{ from: resolve(CONFIG.assetsDir) }]),
        ])
        : commonPlugins.concat([
            new webpack.HotModuleReplacementPlugin(),
        ]),
    resolve: {
        // See https://github.com/fable-compiler/Fable/issues/1490
        symlinks: false
    },
    devServer: {
        publicPath: "/",
        contentBase: resolve(CONFIG.assetsDir),
        port: CONFIG.devServerPort,
        hot: true,
        inline: true
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: CONFIG.babel
                    }
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: CONFIG.babel
                },
            },
            {
                test: /\.(sass|scss|css)$/,
                use: [
                    isProduction
                        ? MiniCssExtractPlugin.loader
                        : 'style-loader',
                    'css-loader',
                    {
                      loader: 'sass-loader',
                      options: { implementation: require("sass") }
                    }
                ],
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)(\?.*)?$/,
                use: ["file-loader"]
            }
        ]
    }
};

function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}
