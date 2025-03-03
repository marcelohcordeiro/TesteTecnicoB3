FROM node:18.13.0 as build

WORKDIR /app

COPY package.json ./
#COPY package.json /app

RUN npm install --force

RUN npm install -g @angular/cli

COPY . .

RUN ng build --configuration=production

FROM nginx:latest

COPY --from=build app/dist/b3.application.front-end /usr/share/nginx/html

EXPOSE 80

#FROM node:21 as builder
#WORKDIR /app
#COPY . .
#RUN npm install
#RUN npm run build
#
#FROM nginx:alpine
#COPY --from=builder /app/dist/login-page/browser /usr/share/nginx/html
#COPY ./nginx.conf /etc/nginx/nginx.conf
#COPY ./mime.types /etc/nginx/mime.types
#EXPOSE 80
#CMD ["nginx", "-g", "daemon off;"]
