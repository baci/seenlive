  module.exports = {

    "env": {

        "browser": true,

        "node": true

    },

    "extends": [
      "prettier",
    ],

    "parser": "@typescript-eslint/parser",

    "parserOptions": {

        "project": "tsconfig.json",

        "sourceType": "module"

    },

    "plugins": [

        "@typescript-eslint",

        "@typescript-eslint/tslint"

    ],

    "ignorePatterns": [".eslintrc.js", "**/__coverage__/**"],

    "rules": {

        "@typescript-eslint/dot-notation": "off",

        "@typescript-eslint/indent": [

            "off",

        ],

        "@typescript-eslint/interface-name-prefix": "off",

        "@typescript-eslint/member-delimiter-style": [

            "error",

            {

                "multiline": {

                    "delimiter": "semi",

                    "requireLast": true

                },

                "singleline": {

                    "delimiter": "semi",

                    "requireLast": false

                }

            }

        ],

        "@typescript-eslint/member-ordering": "error",

        "@typescript-eslint/no-empty-function": "off",

        "@typescript-eslint/no-explicit-any": "off",

        "@typescript-eslint/no-inferrable-types": "off",

        "@typescript-eslint/no-parameter-properties": "off",

        "@typescript-eslint/no-require-imports": "off",

        "@typescript-eslint/no-unused-expressions": [

            "error",

            {

                "allowShortCircuit": true

            }

        ],

        "@typescript-eslint/no-use-before-define": "warn",

        "@typescript-eslint/no-var-requires": "off",

        "@typescript-eslint/prefer-namespace-keyword": "error",

        "@typescript-eslint/quotes": [

            "error",

            "single",

            {

                "avoidEscape": true

            }

        ],

        "@typescript-eslint/semi": [

            "error",

            "always"

        ],

        "@typescript-eslint/type-annotation-spacing": "off",

        "brace-style": [

            "error",

            "1tbs"

        ],

        "camelcase": "off",

        "comma-dangle": "off",

        "curly": "off",

        "default-case": "warn",

        "eol-last": "off",

        "eqeqeq": [

            "error",

            "smart"

        ],

        "guard-for-in": "error",

        "id-blacklist": [

            "off",

            "any",

            "Number",

            "number",

            "String",

            "string",

            "Boolean",

            "boolean",

            "Undefined",

            "undefined"

        ],

        "id-match": "off",

        "max-len": [

            "error",

            {

                "code": 250

            }

        ],

        "no-bitwise": "off",

        "no-caller": "error",

        "no-cond-assign": "error",

        "no-console": [

            "error",

            {

                "allow": [

                    "log",

                    "warn",

                    "dir",

                    "timeLog",

                    "assert",

                    "clear",

                    "count",

                    "countReset",

                    "group",

                    "groupEnd",

                    "table",

                    "debug",

                    "dirxml",

                    "error",

                    "groupCollapsed",

                    "Console",

                    "profile",

                    "profileEnd",

                    "timeStamp",

                    "context"

                ]

            }

        ],

        "no-debugger": "error",

        "no-empty": "off",

        "no-eval": "error",

        "no-fallthrough": "error",

        "no-multiple-empty-lines": "error",

        "no-new-wrappers": "error",

        "no-null/no-null": "off",

        "no-redeclare": "error",

        "no-shadow": [

            "off",

            {

                "hoist": "all"

            }

        ],

        "no-trailing-spaces": "warn",

        "no-underscore-dangle": "off",

        "no-unused-labels": "error",

        "no-var": "error",

        "radix": "error",

        "spaced-comment": [

            "error",

            "always",

            {

                "markers": [

                    "/"

                ]

            }

        ],

        "@typescript-eslint/tslint/config": [

            "error",

            {

                "rules": {

                    "whitespace": [

                        true,

                        "check-branch",

                        "check-decl",

                        "check-operator",

                        "check-separator",

                        "check-type"

                    ]

                }

            }

        ]

    }

};

 