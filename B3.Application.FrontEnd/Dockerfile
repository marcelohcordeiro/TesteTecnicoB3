#FROM node:22-alpine3.19 as builder
#WORKDIR /app
#
## Instalar
#COPY ./package*.json .
#RUN npm install
#
## build
#COPY . .
#RUN npm run build
#
## Enviar o build para o Nginx
#FROM nginx:1.27-alpine as DEPLOY
#COPY ./nginx.conf /etc/nginx/conf.d/default.conf
#
#COPY --from=builder /app/dist/front-angular/browser /usr/share/nginx/html
#EXPOSE 4200
#CMD ["nginx", "-g", "daemon off;"]



#FROM node:alpine
#WORKDIR /usr/src/app
#EXPOSE 4200
#COPY . /usr/src/app
#RUN npm install -g @angular/cli
#RUN npm install
#CMD ["ng", "serve", "--host", "0.0.0.0"]


FROM node:21 as builder
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

FROM nginx:alpine
COPY --from=builder /app/dist/login-page/browser /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY ./mime.types /etc/nginx/mime.types
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
