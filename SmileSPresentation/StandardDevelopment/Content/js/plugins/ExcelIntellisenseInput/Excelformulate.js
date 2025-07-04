; (function ($, window, document, undefined) {
    'use strict'

    var TOKEnum = {
        TOK_TYPE_NOOP: "noop",
        TOK_TYPE_OPERAND: "operand",
        TOK_TYPE_FUNCTION: "function",
        TOK_TYPE_SUBEXPR: "subexpression",
        TOK_TYPE_ARGUMENT: "argument",
        TOK_TYPE_OP_PRE: "operator-prefix",
        TOK_TYPE_OP_IN: "operator-infix",
        TOK_TYPE_OP_POST: "operator-postfix",
        TOK_TYPE_WSPACE: "white-space",
        TOK_TYPE_UNKNOWN: "unknown",

        TOK_SUBTYPE_START: "start",
        TOK_SUBTYPE_STOP: "stop",

        TOK_SUBTYPE_TEXT: "text",
        TOK_SUBTYPE_NUMBER: "number",
        TOK_SUBTYPE_LOGICAL: "logical",
        TOK_SUBTYPE_ERROR: "error",
        TOK_SUBTYPE_RANGE: "range",

        TOK_SUBTYPE_MATH: "math",
        TOK_SUBTYPE_CONCAT: "concatenate",
        TOK_SUBTYPE_INTERSECT: "intersect",
        TOK_SUBTYPE_UNION: "union"
    }

    function ExcelFormula(inp, sel, tulTip, list) {
        this.inp = inp;
        this.sel = sel;
        this.tulTip = tulTip;
        this.list = list;
        this.arrFun = [];
    }

    function f_token(value, type, subtype) {
        this.value = value;
        this.type = type;
        this.subtype = subtype;
    }

    function f_tokens() {
        this.items = new Array();

        this.add = function (value, type, subtype) {
            if (!subtype) subtype = "";
            var token = new f_token(value, type, subtype);
            this.addRef(token);
            return token;
        };

        this.addRef = function (token) {
            this.items.push(token);
        };

        this.index = -1;
        this.reset = function () {
            this.index = -1;
        };
        this.BOF = function () {
            return (this.index <= 0);
        };
        this.EOF = function () {
            return (this.index >= (this.items.length - 1));
        };
        this.moveNext = function () {
            if (this.EOF()) return false;
            this.index++;
            return true;
        };
        this.current = function () {
            if (this.index == -1) return null;
            return (this.items[this.index]);
        };
        this.next = function () {
            if (this.EOF()) return null;
            return (this.items[this.index + 1]);
        };
        this.previous = function () {
            if (this.index < 1) return null;
            return (this.items[this.index - 1]);
        };
    }

    function f_tokenStack() {
        this.items = new Array();

        this.push = function (token) {
            this.items.push(token);
        };
        this.pop = function () {
            var token = this.items.pop();
            return (new f_token("", token.type, TOKEnum.TOK_SUBTYPE_STOP));
        };

        this.token = function () {
            return ((this.items.length > 0) ? this.items[this.items.length - 1] : null);
        };
        this.value = function () {
            return ((this.token()) ? this.token().value : "");
        };
        this.type = function () {
            return ((this.token()) ? this.token().type : "");
        };
        this.subtype = function () {
            return ((this.token()) ? this.token().subtype : "");
        };
    }

    ExcelFormula.prototype = {
        // util
        sortByKey: function (array, key) {
            return array.sort(function (a, b) {
                var x = a[key];
                var y = b[key];
                return ((x < y) ? -1 : ((x > y) ? 1 : 0));
            });
        },

        insertAfter: function (referenceNode, newNode) {
            referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
        },

        getCaretPosition: function () {
            var start, end, ctrl = this.inp;
            if (ctrl.setSelectionRange) {
                start = ctrl.selectionStart;
                end = ctrl.selectionEnd;
            } else if (document.selection && document.selection.createRange) {
                var range = document.selection.createRange();
                start = 0 - range.duplicate().moveStart('character', -100000);
                end = start + range.text.length;
            }
            return {
                start: start,
                end: end
            }
        },

        init: function () {
            this.insertAfter(this.inp, this.tulTip);
            this.insertAfter(this.tulTip, this.sel);
            this.addEvents();
            $(this.sel).hide();
        },

        addEvents: function () {
            var self = this;
            this.inp.addEventListener('keyup', function (e) {
                if (this.value.trim().charAt(0) !== '=') {
                    return false;
                }

                var caret = self.getCaretPosition();
                self.parseFormula(caret.end);
            });

            this.inp.addEventListener('blur', function (e) {
                $(self.tulTip).hide();
                $(self.sel).hide();
            });

            this.inp.focus();
        },

        RenderFormulaDrop: function (str) {
            var ulist, rgxp;

            if (0 === str.length) {
                $(this.sel).hide();
                return false;
            } else {
                ulist = [];
                for (var i = this.list.length - 1; i > -1; --i) {
                    if (this.list[i].value.toLowerCase().indexOf(str) == 0) {
                        ulist.push(this.list[i]);
                    }
                }
            }

            this.updateSelect(this.sortByKey(ulist, 'value'));
        },

        updateSelect: function (arr) {
            var self, opts;
            self = this;
            this.sel.options.length = 0;
            opts = this.buildOpts(arr);
            opts.forEach(function (opt, idx) {
                self.sel.options[idx] = opt;
            });
            opts.length > 0 ? $(self.sel).css('display', 'block') : $(self.sel).hide();
        },

        buildOpts: function (arr) {
            var opts;
            opts = [];
            arr.forEach(function (val) {
                opts.push(new Option(val.value));
            })
            return opts;
        },

        GetFormula: function (formula) {
            var obj = null;
            for (var i = 0; i < this.list.length; i++) {
                if (formula == this.list[i].value.toLowerCase()) {
                    obj = this.list[i];
                    break;
                }
            }
            return obj;
        },

        RenderFormulaTooltip: function () {
            if (typeof this.currFormula === 'undefined') {
                $(this.tulTip).hide();
            } else {
                this.tulTip.style.display = "block";
                var objCurrFormula = this.GetFormula(this.currFormula.value);

                var content = this.getStyledFormulaWithFunArg(objCurrFormula, this.currFormula.argNo);
                $(this.tulTip).html(objCurrFormula.value + "(" + content + ")");
            }
        },

        getStyledFormulaWithFunArg: function (objCurrFormula, currArg) {
            var cfArr = objCurrFormula.argmnts.split(" ");
            if (cfArr.length < currArg + 1)
                return cfArr.join(", ");
            else {
                cfArr[currArg] = "<b>" + cfArr[currArg] + "</b>";
                return cfArr.join(", ");
            }
        },

        parseFormula: function (caretPos) {
            var indentCount = 0;

            var indent = function () {
                var s = "|";
                for (var i = 0; i < indentCount; i++) {
                    s += "&nbsp;&nbsp;&nbsp;|";
                }
                return s;
            };

            var formulaControl = this.inp;
            var formula = formulaControl.value;
            formula = formula.substring(0, caretPos);

            var tokens = this.getTokens(formula);

            var tokensHtml = "";
            tokensHtml += "<table cellspacing='0' style='border-top: 1px #cecece solid; margin-top: 5px; margin-bottom: 5px'>";
            tokensHtml += "<tr>";
            tokensHtml += "<td class='token' style='font-weight: bold; width: 50px'>index</td>";
            tokensHtml += "<td class='token' style='font-weight: bold; width: 125px'>type</td>";
            tokensHtml += "<td class='token' style='font-weight: bold; width: 125px'>subtype</td>";
            tokensHtml += "<td class='token' style='font-weight: bold; width: 150px'>token</td>";
            tokensHtml += "<td class='token' style='font-weight: bold; width: 300px'>token tree</td></tr>";

            this.RenderFormulaTooltip();
            var opValue = '';
            while (tokens.moveNext()) {
                var token = tokens.current();
                if (token.type == TOKEnum.TOK_TYPE_OPERAND) {
                    opValue = token.value;
                }

                if (token.subtype == TOKEnum.TOK_SUBTYPE_STOP)
                    indentCount -= ((indentCount > 0) ? 1 : 0);

                tokensHtml += "<tr>";
                tokensHtml += "<td class='token'>" + (tokens.index + 1) + "</td>";
                tokensHtml += "<td class='token'>" + token.type + "</td>";
                tokensHtml += "<td class='token'>" + ((token.subtype.length == 0) ? "&nbsp;" : token.subtype) + "</td>";
                tokensHtml += "<td class='token'>" + ((token.value.length == 0) ? "&nbsp;" : token.value).split(" ").join("&nbsp;") + "</td>";
                tokensHtml += "<td class='token'>" + indent() + ((token.value.length == 0) ? "&nbsp;" : token.value).split(" ").join("&nbsp;") + "</td>";
                tokensHtml += "</tr>";
                if (token.subtype == TOKEnum.TOK_SUBTYPE_START) {
                    indentCount += 1;
                }
            }

            this.RenderFormulaDrop(opValue);

            tokensHtml += "</table>";

            document.getElementById("tokens").innerHTML = tokensHtml;

            formulaControl.focus();
        },

        getTokens: function (formula) {
            var tokens = new f_tokens();
            var tokenStack = new f_tokenStack();

            var offset = 0;

            var currentChar = function () {
                return formula.substr(offset, 1);
            };
            var doubleChar = function () {
                return formula.substr(offset, 2);
            };
            var nextChar = function () {
                return formula.substr(offset + 1, 1);
            };
            var EOF = function () {
                return (offset >= formula.length);
            };

            var token = "";

            var inString = false;
            var inPath = false;
            var inRange = false;
            var inError = false;

            while (formula.length > 0) {
                if (formula.substr(0, 1) == " ")
                    formula = formula.substr(1);
                else {
                    if (formula.substr(0, 1) == "=")
                        formula = formula.substr(1);
                    break;
                }
            }

            var regexSN = /^[1-9]{1}(\.[0-9]+)?E{1}$/;

            while (!EOF()) {
                // state-dependent character evaluation (order is important)

                // double-quoted strings
                // embeds are doubled
                // end marks token

                if (inString) {
                    if (currentChar() == "\"") {
                        if (nextChar() == "\"") {
                            token += "\"";
                            offset += 1;
                        } else {
                            inString = false;
                            tokens.add(token, TOKEnum.TOK_TYPE_OPERAND, TOKEnum.TOK_SUBTYPE_TEXT);
                            token = "";
                        }
                    } else {
                        token += currentChar();
                    }
                    offset += 1;
                    continue;
                }

                // single-quoted strings (links)
                // embeds are double
                // end does not mark a token

                if (inPath) {
                    if (currentChar() == "'") {
                        if (nextChar() == "'") {
                            token += "'";
                            offset += 1;
                        } else {
                            inPath = false;
                        }
                    } else {
                        token += currentChar();
                    }
                    offset += 1;
                    continue;
                }

                // bracked strings (range offset or linked workbook name)
                // no embeds (changed to "()" by Excel)
                // end does not mark a token

                if (inRange) {
                    if (currentChar() == "]") {
                        inRange = false;
                    }
                    token += currentChar();
                    offset += 1;
                    continue;
                }

                // error values
                // end marks a token, determined from absolute list of values

                if (inError) {
                    token += currentChar();
                    offset += 1;
                    if ((",#NULL!,#DIV/0!,#VALUE!,#REF!,#NAME?,#NUM!,#N/A,").indexOf("," + token + ",") != -1) {
                        inError = false;
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND, TOKEnum.TOK_SUBTYPE_ERROR);
                        token = "";
                    }
                    continue;
                }

                // scientific notation check

                if (("+-").indexOf(currentChar()) != -1) {
                    if (token.length > 1) {
                        if (token.match(regexSN)) {
                            token += currentChar();
                            offset += 1;
                            continue;
                        }
                    }
                }

                // independent character evaulation (order not important)

                // establish state-dependent character evaluations

                if (currentChar() == "\"") {
                    if (token.length > 0) {
                        // not expected
                        tokens.add(token, TOKEnum.TOK_TYPE_UNKNOWN);
                        token = "";
                    }
                    inString = true;
                    offset += 1;
                    continue;
                }

                if (currentChar() == "'") {
                    if (token.length > 0) {
                        // not expected
                        tokens.add(token, TOKEnum.TOK_TYPE_UNKNOWN);
                        token = "";
                    }
                    inPath = true;
                    offset += 1;
                    continue;
                }

                if (currentChar() == "[") {
                    inRange = true;
                    token += currentChar();
                    offset += 1;
                    continue;
                }

                if (currentChar() == "#") {
                    if (token.length > 0) {
                        // not expected
                        tokens.add(token, TOKEnum.TOK_TYPE_UNKNOWN);
                        token = "";
                    }
                    inError = true;
                    token += currentChar();
                    offset += 1;
                    continue;
                }

                // mark start and end of arrays and array rows

                if (currentChar() == "{") {
                    if (token.length > 0) {
                        // not expected
                        tokens.add(token, TOKEnum.TOK_TYPE_UNKNOWN);
                        token = "";
                    }
                    tokenStack.push(tokens.add("ARRAY", TOKEnum.TOK_TYPE_FUNCTION, TOKEnum.TOK_SUBTYPE_START));
                    tokenStack.push(tokens.add("ARRAYROW", TOKEnum.TOK_TYPE_FUNCTION, TOKEnum.TOK_SUBTYPE_START));
                    offset += 1;
                    continue;
                }

                if (currentChar() == ";") {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.addRef(tokenStack.pop());
                    tokens.add(",", TOKEnum.TOK_TYPE_ARGUMENT);
                    tokenStack.push(tokens.add("ARRAYROW", TOKEnum.TOK_TYPE_FUNCTION, TOKEnum.TOK_SUBTYPE_START));
                    offset += 1;
                    continue;
                }

                if (currentChar() == "}") {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.addRef(tokenStack.pop());
                    tokens.addRef(tokenStack.pop());
                    offset += 1;
                    continue;
                }

                // trim white-space

                if (currentChar() == " ") {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.add("", TOKEnum.TOK_TYPE_WSPACE);
                    offset += 1;
                    while ((currentChar() == " ") && (!EOF())) {
                        offset += 1;
                    }
                    continue;
                }

                // multi-character comparators

                if ((",>=,<=,<>,").indexOf("," + doubleChar() + ",") != -1) {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.add(doubleChar(), TOKEnum.TOK_TYPE_OP_IN, TOKEnum.TOK_SUBTYPE_LOGICAL);
                    offset += 2;
                    continue;
                }

                // standard infix operators

                if (("+-*/^&=><").indexOf(currentChar()) != -1) {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.add(currentChar(), TOKEnum.TOK_TYPE_OP_IN);
                    offset += 1;
                    continue;
                }

                // standard postfix operators

                if (("%").indexOf(currentChar()) != -1) {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.add(currentChar(), TOKEnum.TOK_TYPE_OP_POST);
                    offset += 1;
                    continue;
                }

                // start subexpression or function

                if (currentChar() == "(") {
                    if (token.length > 0) {
                        tokenStack.push(tokens.add(token, TOKEnum.TOK_TYPE_FUNCTION, TOKEnum.TOK_SUBTYPE_START));
                        token = "";
                    } else {
                        tokenStack.push(tokens.add("", TOKEnum.TOK_TYPE_SUBEXPR, TOKEnum.TOK_SUBTYPE_START));
                    }
                    offset += 1;
                    continue;
                }

                // function, subexpression, array parameters

                if (currentChar() == ",") {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    if (!(tokenStack.type() == TOKEnum.TOK_TYPE_FUNCTION)) {
                        tokens.add(currentChar(), TOKEnum.TOK_TYPE_OP_IN, TOKEnum.TOK_SUBTYPE_UNION);
                    } else {
                        tokens.add(currentChar(), TOKEnum.TOK_TYPE_ARGUMENT);
                    }
                    offset += 1;
                    continue;
                }

                // stop subexpression

                if (currentChar() == ")") {
                    if (token.length > 0) {
                        tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);
                        token = "";
                    }
                    tokens.addRef(tokenStack.pop());
                    offset += 1;
                    continue;
                }

                // token accumulation

                token += currentChar();
                offset += 1;
            }

            // dump remaining accumulation

            if (token.length > 0) tokens.add(token, TOKEnum.TOK_TYPE_OPERAND);

            // move all tokens to a new collection, excluding all unnecessary white-space tokens

            var tokens2 = new f_tokens();

            while (tokens.moveNext()) {
                token = tokens.current();

                if (token.type == TOKEnum.TOK_TYPE_WSPACE) {
                    if ((tokens.BOF()) || (tokens.EOF())) { } else if (!(
                        ((tokens.previous().type == TOKEnum.TOK_TYPE_FUNCTION) && (tokens.previous().subtype == TOKEnum.TOK_SUBTYPE_STOP)) ||
                        ((tokens.previous().type == TOKEnum.TOK_TYPE_SUBEXPR) && (tokens.previous().subtype == TOKEnum.TOK_SUBTYPE_STOP)) ||
                        (tokens.previous().type == TOKEnum.TOK_TYPE_OPERAND)
                    )) { } else if (!(
                        ((tokens.next().type == TOKEnum.TOK_TYPE_FUNCTION) && (tokens.next().subtype == TOKEnum.TOK_SUBTYPE_START)) ||
                        ((tokens.next().type == TOKEnum.TOK_TYPE_SUBEXPR) && (tokens.next().subtype == TOKEnum.TOK_SUBTYPE_START)) ||
                        (tokens.next().type == TOKEnum.TOK_TYPE_OPERAND)
                    )) { } else
                        tokens2.add(token.value, TOKEnum.TOK_TYPE_OP_IN, TOKEnum.TOK_SUBTYPE_INTERSECT);
                    continue;
                }

                tokens2.addRef(token);
            }

            // switch infix "-" operator to prefix when appropriate, switch infix "+" operator to noop when appropriate, identify operand
            // and infix-operator subtypes, pull "@" from in front of function names

            while (tokens2.moveNext()) {
                token = tokens2.current();

                if ((token.type == TOKEnum.TOK_TYPE_OP_IN) && (token.value == "-")) {
                    if (tokens2.BOF())
                        token.type = TOKEnum.TOK_TYPE_OP_PRE;
                    else if (
                        ((tokens2.previous().type == TOKEnum.TOK_TYPE_FUNCTION) && (tokens2.previous().subtype == TOKEnum.TOK_SUBTYPE_STOP)) ||
                        ((tokens2.previous().type == TOKEnum.TOK_TYPE_SUBEXPR) && (tokens2.previous().subtype == TOKEnum.TOK_SUBTYPE_STOP)) ||
                        (tokens2.previous().type == TOKEnum.TOK_TYPE_OP_POST) ||
                        (tokens2.previous().type == TOKEnum.TOK_TYPE_OPERAND)
                    )
                        token.subtype = TOKEnum.TOK_SUBTYPE_MATH;
                    else
                        token.type = TOKEnum.TOK_TYPE_OP_PRE;
                    continue;
                }

                if ((token.type == TOKEnum.TOK_TYPE_OP_IN) && (token.value == "+")) {
                    if (tokens2.BOF())
                        token.type = TOKEnum.TOK_TYPE_NOOP;
                    else if (
                        ((tokens2.previous().type == TOKEnum.TOK_TYPE_FUNCTION) && (tokens2.previous().subtype == TOKEnum.TOK_SUBTYPE_STOP)) ||
                        ((tokens2.previous().type == TOKEnum.TOK_TYPE_SUBEXPR) && (tokens2.previous().subtype == TOKEnum.TOK_SUBTYPE_STOP)) ||
                        (tokens2.previous().type == TOKEnum.TOK_TYPE_OP_POST) ||
                        (tokens2.previous().type == TOKEnum.TOK_TYPE_OPERAND)
                    )
                        token.subtype = TOKEnum.TOK_SUBTYPE_MATH;
                    else
                        token.type = TOKEnum.TOK_TYPE_NOOP;
                    continue;
                }

                if ((token.type == TOKEnum.TOK_TYPE_OP_IN) && (token.subtype.length == 0)) {
                    if (("<>=").indexOf(token.value.substr(0, 1)) != -1)
                        token.subtype = TOKEnum.TOK_SUBTYPE_LOGICAL;
                    else if (token.value == "&")
                        token.subtype = TOKEnum.TOK_SUBTYPE_CONCAT;
                    else
                        token.subtype = TOKEnum.TOK_SUBTYPE_MATH;
                    continue;
                }

                if ((token.type == TOKEnum.TOK_TYPE_OPERAND) && (token.subtype.length == 0)) {
                    if (isNaN(parseFloat(token.value)))
                        if ((token.value == 'TRUE') || (token.value == 'FALSE'))
                            token.subtype = TOKEnum.TOK_SUBTYPE_LOGICAL;
                        else
                            token.subtype = TOKEnum.TOK_SUBTYPE_RANGE;
                    else
                        token.subtype = TOKEnum.TOK_SUBTYPE_NUMBER;
                    continue;
                }

                if (token.type == TOKEnum.TOK_TYPE_FUNCTION) {
                    if (token.value.substr(0, 1) == "@")
                        token.value = token.value.substr(1);
                    continue;
                }
            }

            tokens2.reset();

            // move all tokens to a new collection, excluding all noops

            tokens = new f_tokens();
            var currFormulaArr = [],
                argNo = 0;
            while (tokens2.moveNext()) {
                var currToken = tokens2.current();
                if (currToken.type != TOKEnum.TOK_TYPE_NOOP) {
                    tokens.addRef(currToken);

                    if (currToken.type == TOKEnum.TOK_TYPE_FUNCTION) {
                        if (currToken.subtype == TOKEnum.TOK_SUBTYPE_START) {
                            argNo = 0;
                            currFormulaArr.push({
                                'value': currToken.value,
                                'argNo': argNo
                            });
                        } else { //function STOP condition
                            currFormulaArr.pop();
                            if (currFormulaArr.length > 0)
                                argNo = currFormulaArr[currFormulaArr.length - 1].argNo;
                        }
                    } else if (currToken.type == TOKEnum.TOK_TYPE_ARGUMENT) {
                        argNo++;
                        currFormulaArr[currFormulaArr.length - 1].argNo = argNo;
                    }
                }
            }
            this.currFormula = currFormulaArr.pop(); // added new attribute to excelFormula

            tokens.reset();

            return tokens;
        }
    }

    $.fn.ExcelFormulate = function (options) {
        var defaults = {
            textColor: "#000",
            backgroundColor: "#fff"
        };

        var settings = $.extend({}, defaults, options);
        return this.each(function () {
            $(this).css({
                'color': settings.textColor,
                'background-color': settings.backgroundColor
            });

            // initialization
            var inp = this;
            var sel = document.createElement("select");
            sel.id = inp.id + '_formulaeList';
            sel.multiple = true;
            var tulTip = document.createElement('div');
            tulTip.id = inp.id + '_exceltooltip';
            tulTip.className = 'exltooltip';
            var list = (function () {
                var excelFormulaeSource = [{
                    value: "AND",
                    tip: "Checks whether AND condition is met, blah blah blah...",
                    argmnts: "Expression1 Expression2"
                }, {
                    value: "IF",
                    tip: "Checks whether IF condition is met, blah blah blah...",
                    argmnts: "Expression value_if_true value_if_false"
                }, {
                    value: "IFERROR",
                    tip: "Checks whether IFError condition is met, blah blah blah...",
                    argmnts: "value value_if_error"
                }];

                return excelFormulaeSource;
            }());

            var tbExel = new ExcelFormula(inp, sel, tulTip, list);
            tbExel.init();
        });
    }
})(jQuery, window, document);