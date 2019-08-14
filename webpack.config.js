const path = require('path');

module.exports = env => {
    const entry = env.entry;
    return {
        entry: entry,
        output: {
            path: path.join(__dirname, './public'),
            filename: 'bundle.js'
        },
        devServer: {
            contentBase: './public',
            port: 8888
        },
        module: {
            rules: [{
                test: /\.fs(x|proj)?$/,
                use: 'fable-loader'
            }]
        }
    }
};