import svelte from 'rollup-plugin-svelte';
import commonjs from '@rollup/plugin-commonjs';
import resolve from '@rollup/plugin-node-resolve';
import livereload from 'rollup-plugin-livereload';
import { terser } from 'rollup-plugin-terser';
import css from 'rollup-plugin-css-only';
import multiInput from 'rollup-plugin-multi-input';
import fs from "fs";

const production = !process.env.ROLLUP_WATCH;
const entryPointDirectory = "./integrations"
function serve() {
	let server;

	function toExit() {
		if (server) server.kill(0);
	}

	return {
		writeBundle() {
			if (server) return;
			server = require('child_process').spawn('npm', ['run', 'start', '--', '--dev'], {
				stdio: ['ignore', 'inherit', 'inherit'],
				shell: true
			});

			process.on('SIGTERM', toExit);
			process.on('exit', toExit);
		}
	};
}

function readEntryPointFiles() {
	var arr = [];
	var files = fs.readdirSync(entryPointDirectory);
	var len = files.length;

	for (var i = 0; i < len; i++){
		if (files[i].endsWith(".js")) arr.push(`${entryPointDirectory}/${files[i]}`);
	}

	return arr;
}

export default {
	input: readEntryPointFiles(),
	output: {
		format: 'esm',
		dir: '../wwwroot'
	},
	plugins: [
		multiInput(),
		svelte({
			compilerOptions: {
				// enable run-time checks when not in production
				dev: !production
			}
		}),
		// we'll extract any component CSS out into
		// a separate file - better for performance
		css({ output: function(styles) {
			fs.writeFileSync("../wwwroot/css/bundle.css", styles);
		} }),

		// If you have external dependencies installed from
		// npm, you'll most likely need these plugins. In
		// some cases you'll need additional configuration -
		// consult the documentation for details:
		// https://github.com/rollup/plugins/tree/master/packages/commonjs
		resolve({
			browser: true,
			dedupe: ['svelte']
		}),
		commonjs(),

		// In dev mode, call `npm run start` once
		// the bundle has been generated
		!production && serve(),

		// Watch the `public` directory and refresh the
		// browser on changes when not in production
		!production && livereload('public'),

		// If we're building for production (npm run build
		// instead of npm run dev), minify
		production && terser()
	],
	watch: {
		clearScreen: false
	}
};
