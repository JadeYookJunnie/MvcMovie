### To add node modules for sass
npm install 
### To delete node modules 
rm -rf node_modules
#### Compile scss to css
sass src/scss/sass.scss wwwroot/css/sass.css 

#### Run sass watch to see if any edit are correct
npm run sass

### Hosting failed to start 
lsof -i :5122
kill -9 <PID>
