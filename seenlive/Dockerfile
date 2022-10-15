# DEVELOPMENT STAGE
FROM node:14 AS development

# Create app directory
WORKDIR /usr/src/app

# Install app dependencies
COPY package*.json ./
COPY yarn.lock ./
RUN yarn install

# Bundle app source
COPY . .

EXPOSE 3000
EXPOSE 3001
CMD [ "yarn", "start-dev" ]

##################
# PRODUCTION STAGE
FROM node:14 AS production

# Create app directory
WORKDIR /usr/src/app

# Install app dependencies
COPY package*.json ./
COPY yarn.lock ./
RUN yarn install

# Bundle app source
COPY . .

EXPOSE 3000
EXPOSE 3001
CMD [ "yarn", "start-prod" ]