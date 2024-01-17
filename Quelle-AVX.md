# Quelle Specification for AVX Framework

##### AVX-Quelle version 2.4.117

### I. Background

Most modern search engines, provide a mechanism for searching via a text input box, where the user is expected to type search terms. While this interface is primitive from a UI perspective, it facilitates rich augmentation via search-specific semantics. Google pioneered the modern search interface by employing an elegantly simple "search box". This was an evolution away from the complex interfaces that preceded it. However, when a user searches for multiple terms, it becomes confusing how to obtain any results except "match every term".

The vast world of search is rife for a standardized search-syntax that moves us past only basic search capabilities. Without the introduction of a complicated search UI, Quelle represents a freely available specification for an open Human-Machine-Interface (HMI). It can be easily invoked from within a simple text input box on a web page or even from a specialized command shell. The syntax supports Boolean operations such as AND, OR, and NOT, albeit in a non-gear-headed way. While great care has been taken to support the construction of complex queries, greater care has been taken to maintain a clear and concise syntax.

Quelle, IPA: [kɛl], in French means "What? or Which?". As Quelle HMI is designed to obtain search-results from search-engines, this interrogative nature befits its name. An earlier interpreter, Clarity, served as inspiration for defining Quelle.  You could think of the Quelle HMI as the next generation of Clarity HMI.  Yet, in order to produce linguistic consistency, Quelle syntax varied dramatically from the Clarity spec. Therefore, a new name was the best way forward.  Truly, Quelle HMI incorporates lessons learned after creating, implementing, and revising Clarity HMI for over a decade.

In 2023, Quelle 2.0 was released. This new release is not a radical divergence from version 1. Most of the updates to the specification are related to macros, control variables, filters, and export directives. Search syntax has remained largely unchanged. We discovered some ambiguity in the PEG grammar that implements the Quelle parser. Certain implicit actions could not be defined deterministically. To resolve those ambiguities, Quelle syntax was refined and updated. Consequently, new operators were introduced  [We added # $ % and || to name a few] . These new operators eliminate the need for clause delimiters, in most circumstances. As a result, the version 2 syntax is more streamlined and more intuitive. It comes with a reference implementation in Rust and a fully specified PEG grammar.  Implicit actions for Macros are now referred to as *apply* and *invoke* [those verbs replace *save* and *execute* respectively].

Quelle is consistent with itself, to make it feel intuitive. Some constructs make parsing unambiguous; other constructs increase ease of typing (Specifically, we attempt to minimize the need to press the shift-key). Naturally, existing scripting languages also influence our syntax. Quelle represents an easy to type and easy to learn HMI.  Moreover, simple search statements look no different than they might appear today in a Google or Bing search box. Still, let's not get ahead of ourselves or even hint about where our simple specification might take us ;-)

Finally, Quelle 2.0 has dropped support for "bracketed search terms". Bracketed search terms allowed some terms within a quoted string to be unordered. Bracketing signaled that those terms could be in any order. Issues surfaced when defining the blueprint-blue object model. Some terms needed to be in a specific order and others did not, all within the same search construct. To make matters worse, the construct was seldom used. Notwithstanding, bracketed terms were even difficult to explain. Rather than tackle these problems for such a minor feature, we elected to nix bracketed terms altogether in Quelle.

### II. Addendum

Vanilla Quelle specifies two possible implementation levels:

- Level 1 [basic search support]
- Level 2 [search support includes also searching on part-of-speech tags]

AVX-Quelle is a Level 2 implementation with augmented search capabilities. AVX-Quelle extends baseline Vanilla-Quelle to include AVX-specific constructs.  These extensions provide additional specialized search features and the ability to manage two distinct lexicons for the biblical texts.

1. AVX-Quelle represents the biblical text with two substantially similar, but distinct, lexicons. The %lexicon setting can be specified by the user to control which lexicon is to be searched. Likewise, the %display setting is used to control which lexicon is used for displaying the biblical text. As an example, the KJV text of "thou art" would be modernized to "you are".

   - AV/KJV *(a lexicon that faithfully represents the KJV bible; AV purists should select this setting)*

   - AVX/Modern *(a lexicon that that has been modernized to appear more like contemporary English)*

   - Dual/Both *(use both lexicons for searching; this setting is not compatible with the %display setting)*

2. AVX-Quelle provides support for fuzzy-match-logic. The %similarity setting can be specified by the user to control the similarity threshold for approximate matching. An exact lexical match is expected when %similarity is set to *exact* or 0.  Zero is not really a similarity threshold, but rather 0 is a synonym for *exact*.

   Approximate matches are considered when similarity is set between 33 and 99 (33% to 99%). Similarity is calculated based upon the phonetic representation for the word.

   The minimum permitted similarity threshold is 33%. Any similarity threshold between 1 and 32 produces a syntax error.

   A %similarity setting of *precise* or 100 is a special case that still uses phonetics, but expects a full phonetic match (e.g. "there" and "their" are a 100% phonetic match).

3. In a future release, similarity matching on lemmas can also be specified. Similarity matching on lemmas will be enabled by appending an exclamation mark ( ! ) to the similarity threshold (e.g. %similarity = 75!). Automatic matching on lemmas is not supported when %similarity is set to *exact* or 0. The PEG grammar already supports the exclamation mark in the syntax. However, it is currently ignored in AVX-Search. A future version of AVX-Search will utilize this syntax to enable this feature.

AVX-Quelle uses the AV-1769 edition of the sacred text. It substantially agrees with the "Bearing Precious Seed" bibles, as published by local church ministries. The text itself has undergone review by Christian missionaries, pastors, and lay people since the mid-1990's. The original incarnation of the digitized AV-1769 text was implemented in the free PC/Windows app known as:

- AV-1995
- AV-1997
- AV-1999
- AV-2000
- AV-2007
- AV-2011
- AV-Bible for Windows

These releases were found at the [older/legacy] avbible.net website. Initially [decades ago], these releases were found on internet bulletin boards and the [now defunct] bible.advocate.com website.

Please see https://Digital-AV.org for additional information about the SDK.

### III. Quelle Syntax

Quelle defines a declarative syntax for specifying search criteria using the *find* verb. Quelle also defines additional verbs to round out its syntax as a simple straightforward means to interact with custom applications where searching text is the fundamental problem at hand.

Quelle Syntax comprises fifteen (15) verbs. Each verb corresponds to a basic action:

- find
- filter
- assign
- set
- get
- clear
- history
- print
- apply
- delete
- absorb
- help
- review
- invoke
- exit

Quelle is an open and extensible standard, additional verbs, and verbs for other languages can be defined without altering the overall syntax structure of the Quelle HMI. The remainder of this document describes Version 2.0 of the Quelle-HMI Level-II specification with specialized AVX search augmentations.  Moreover, there is no need to consult the Vanilla-Quelle documentation, as all similarities with Vanilla-Quelle are redundantly documented here.

In Quelle terminology, a statement is made up of one or more clauses. Each clause represents an action. While there are fifteen action-verbs, there are only five syntax categories:

1. SEARCH
   - *find*
   - *filter*
2. CONTROL
   - *assign*
   - @set
   - @clear
   - @get
   - @absorb *(use history **or** label to **absorb all control settings**)*
4. OUTPUT
   - @print
4. SYSTEM
   - @help
   - @exit
5. HISTORY & MACROS
   - *invoke*			  (invoke macro by its label **or** invoke a previous command by its id)
   - *apply*			   (apply label to define a macro)
   - @delete		  (delete macro by its label)
   - @review		 (review macros)
   - @history	  (review history or reset history)

Each clause has either a single explicit action or any number of implicit actions.  Explicit actions begin with the @ symbol, immediately followed by the explicit verb.  Implicit actions are inferred by the syntax of the command.

### IV. Fundamental Quelle Commands

Learning just six verbs is all that is necessary to effectively use Quelle. In the table below, each verb is identified with required and optional parameters/operators.

| Verb       | Action Type | Syntax Category | Required Parameters     |              Optional Parameters               |
| ---------- | :---------: | :-------------- | ----------------------- | :--------------------------------------------: |
| *find*     |  implicit   | SEARCH          | *search spec*           |                                                |
| *filter*   |  implicit   | SEARCH          | **<** *domain*          |                                                |
| *assign*   |  implicit   | CONTROL         | **%name** **=** *value* |                                                |
| **@print** |  explicit   | OUTPUT          | $* or other $label      | *book, chapter, verse and/or export directive* |
| **@help**  |  explicit   | SYSTEM          |                         |                    *topic*                     |
| **@exit**  |  explicit   | SYSTEM          |                         |                                                |

**TABLE 4-1** -- **Fundamental Quelle commands with corresponding syntax summaries**

From a linguistic standpoint, all Quelle commands are issued in the imperative. The subject of the verb is always "you understood". As the user, you are commanding Quelle what to do. Some verbs have direct objects [aka required parameters]. These parameters instruct Quelle <u>what</u> to act upon. The verb dictates the required parameters: in linguistic terms, this is referred to as the valence of the verb.

Quelle supports two types of actions:

1. Implicit actions [implicit actions are inferred from the syntax of their parameters]
2. Explicit actions [The verb needs to be explicitly stated in the command and it begins with **@**]

Implicit actions can be combined into compound statements.  However, compound statements are limited to contain ONLY implicit actions. This means that explicit actions cannot be used to construct a compound statement.

As search is a fundamental concern of Quelle, it is optimized to make compound implicit actions easy to construct with a concise and intuitive syntax. Even before we describe Quelle syntax generally, let's examine a few concepts using examples:

| Description                                 | Example                                  |
| ------------------------------------------- | :--------------------------------------- |
| SYSTEM command                              | @help                                    |
| SEARCH filters                              | < Genesis < Exodus < Revelation          |
| SEARCH specification                        | this is some text expected to be found   |
| Compound statement: two SEARCH exxpressions | "this quoted text" + other unquoted text |
| Compound statement: two CONTROL assignments | %span = 7 %similarity = 85               |
| Compound statement: CONTROL & SEARCH        | %span = 7 Moses said                     |

**TABLE 4-2** -- **Examples of Quelle statement types**

Consider these two examples of Quelle statements (first CONTROL; then SEARCH):

@set %lexicon = KJV

"Moses"

Notice that both statements above are single actions.  We should have a way to express both of these in a single command. And this is the rationale behind a compound statement. To combine the previous two actions into one compound statement, issue this command:

"Moses" %lexicon=KJV

### V. Deep Dive into Quelle SEARCH actions

Consider this proximity search where the search using Quelle syntax:

*%span=7  Moses Aaron*

Quelle syntax can define the lexicon by also supplying an additional CONTROL action:

*%span=7 %lexicon=KJV  Moses Aaron*

The statement above has two CONTROL actions and one SEARCH action

Next, consider a search to find Moses or Aaron:

*Moses|Aaron*

The order in which the search terms are provided is insignificant. Additionally, the type-case is insignificant. 

Of course, there are times when word order is significant. Accordingly, searching for explicit strings can be accomplished using double-quotes as follows:

*"Moses said ... Aaron"*

These constructs can even be combined. For example:

*"Moses said ... Aaron|Miriam"*

The search criteria above is equivalent to this search:

*"Moses said ... Aaron" + "Moses said ... Miriam"*

In all cases, “...” means “followed by”, but the ellipsis allows other words to appear between "said" and "Aaron". Likewise, it allows words to appear between "said" and "Miriam". 

Quelle is designed to be intuitive. It provides the ability to invoke Boolean logic for term-matching and/or linguistic feature-matching. As we saw above, the pipe symbol ( | ) can be used to invoke an *OR* condition In effect, this invokes Boolean multiplication on the terms and features that compose the expression.

The ampersand symbol can similarly be used to represent *AND* conditions upon terms. As an example. the English language contains words that can sometimes as a noun , and other times as a noun or other part-of-speech. To determine if the bible text contains the word "part" where it is used as a verb, we can issue this command:

"part&/verb/"

The SDK, provided by Digital-AV, has marked each word of the bible text for part-of-speech. With Quelle's rich syntax, this type of search is easy and intuitive.

Of course, part-of-speech expressions can also be used independently of an AND condition, as follows:

%span = 6  "/noun/ ... home"

That search would find phrases where a noun appeared within a span of six words, preceding the word "home"

**Valid statement syntax, but no results:**

this&that

/noun/ & /verb/

Both of the statements above are valid, but will not match any results. Search statements attempt to match actual words in  the actual bible text. A word cannot be "this" **and** "that". Likewise, an individual word in a sentence does not operate as a /noun/ **and** a /verb/.

**Negating search-terms Example:**

Consider a query for all passages that contain a word beginning with "Lord", followed by any word that is neither a verb nor an adverb:

%span = 15 "Lord\* -/v/ & -/adv/"

this|that

/noun/ | /verb/

### VI. Printing Results

Consider that there are two fundamental types of searches:

- Searches that return a limited set of results
- Searches that either return loads of results; or searches where the result count is unknown (and potentially very large)

Due to the latter condition above, SEARCH summarizes results (it does NOT automatically display every result found). However, when more than a summary is needed, the explicit @print command controls verse rendering. printing results can be directly to the console window or further controlled with the export directive.

### VII. The Export directive

This would export a summary for the most recent search

@print $* > my-search-summary.output  

This would export all found verses in Genesis from the most previous search as html

@set %format=html

@print $* Genesis  > my-search-genesis.output.html

This would export all verses in Genesis, independent of any search criteria) as markdown

@set %format=md

@print Genesis:1  > gen.md

To append Genesis chapter two to the previous chapter one output, use >>

@print Genesis:2  >> gen.md

To overwrite the previous file to only contain chapter 50 use =>

@print Genesis:50  => gen.md



| Directive | Action Type | Syntax Category | Parameters       | Alternate #1      | Alternate #2      |
| --------- | :---------: | --------------- | ---------------- | :---------------- | :---------------- |
| *export*  |    print    | OUTPUT          | **>** *filename* | **=>** *filename* | **>>** *filename* |

**TABLE 7-1** -- **The export directive**

### VIII. Filtering Results

Sometimes we want to constrain the domain of where we are searching. Say that I want to find mentions of the serpent in Genesis. I can search only Genesis by executing this search:

serpent < Genesis

If I also want to search in Genesis and Revelation, this works:

serpent < Genesis < Revelation

Filters do not allow spaces, but they do allow Chapter and Verse specifications. To search for the serpent in Genesis Chapter 3, we can do this:

serpent < Genesis:3

And books that contain spaces are supported by eliminating the spaces. For example, this is a valid command:

vanity < SongOfSolomon < 1Corinthians

Abbreviations are also supported:

vanity < sos < 1co

### IX. Labeling & Reviewing Statements for subsequent invocation

| Verb         | Action Type | Syntax Category           | Parameters                                                   |
| ------------ | ----------- | ------------------------- | ------------------------------------------------------------ |
| *invoke*     | implicit    | <u>HISTORY</u> & LABELING | ***$**id* or ***#**id*                                       |
| *invoke*     | implicit    | HISTORY & <u>LABELING</u> | ***$**label* or ***#**label*                                 |
| *apply*      | implicit    | HISTORY & <u>LABELING</u> | **\|\|** ***$**label*<br/>\<or\><br/> **\|\|** ****#**label** |
| **@delete**  | explicit    | HISTORY & <u>LABELING</u> | ***$**label* or ***#**label*                                 |
| **@review**  | explicit    | HISTORY & <u>LABELING</u> | **optional:** ***$**label* or ***#**label* or wildcard<br/>**optional:** < yyyy/mm/dd<br/>**optional:** > yyyy/mm/dd |
| **@absorb**  | explicit    | CONTROL                   | **permitted:** ***$**label* or ***#**label* or  ***$**id* or  ***#**id* |
| **@history** | explicit    | <u>HISTORY</u> & LABELING | **optional:** ***$**id* or ***$**id*<br/>**optional:** < yyyy/mm/dd<br/>**optional:** > yyyy/mm/dd<br/>**optional:** < id<br/>**optional:** > id<br/>**optional:** -reset<br>**optional:** -reset \< *id*<br/>**optional:** -reset \> *id*<br/>**optional:** -reset < yyyy/mm/dd<br/>**optional:** -reset > yyyy/mm/dd |

**TABLE 9-1** -- **Labeling statements and reviewing statement history**

There are two types of macros:

- **FULL** macros that save the search expression and the settings & filters
- **PARTIAL** macros that save only settings & filters

Full-macros are defined with the dollar sign ($); partial macros are defined with the (#). Partial macros are useful, as they provide more liberal utilization of previously labelled statements. Quelle search syntax allows only a single search expression: this limits the application of full macros. partial-macro invocations are not restricted in that manner.  Full macro can be demoted to a partial macro by using a # instead of $ upon invocation. Partial macros can be promoted to full macros, by redefining adding a search expression and redefining the macro. 

In this section, we will examine Quelle syntax, and how macros can be used to label statements for subsequent invocation.  By applying a label to a statement, a shorthand mechanism is created for subsequent invocation. This gives rise to two new definitions:

1. Labeling a statement (or defining a macro)

2. Invoking a labeled statement (running a macro)

Macro labels cannot contain punctuation: only letters, numbers, hyphens, and underscores are permitted. However, macros are identified with a prefix: it is either a pound (#) or dollar ($).


Let’s say we want to name the search example from the previous section; We’ll call it *eternal-power*. To accomplish this, we can apply a label to the statement below. This produces a full macro:

%span=7 %similarity=85 eternal power < Romans || $eternal-power-romans

It’s that simple, now instead of typing the entire statement, we can utilize the macro by referencing our previously applied label. Here is how the macro can be invoked:

$eternal-power-romans

Labeled statements also support compounding. However, statements can only ever have a single search expression. Therefore, this macro invocation is disallowed (as there would be two search expressions in the same segment):

$eternal-power godhead

However, the control variables and filters that were in the macro can still be leveraged by demoting the label to a partial macro. We can execute this to accomplish the search that was disallowed above:

#eternal-power-romans eternal power godhead

Alternatively, the macro can be redefined (his example also release the syntax for promoting a partial macro to a full macro:

#eternal-power-romans eternal power godhead || $godhead-romans

There are a few restrictions on macro definitions:

1. The statement cannot represent an explicit action:
   - Only implicit actions are permitted in a labeled statement.
2.  definition must represent a valid Quelle statement:
   - The syntax is verified prior to applying the statement label.
3. Macro definitions apply per segment
   - when + is used to concatenate searches, the macro applies only to the single expression immediately to its left

Finally, any macros referenced within a macro definition are expanded prior to applying the new label. Therefore, subsequent redefinition of a previously referenced macro invocation has no effect upon the initial macro reference. We call this macro-determinism.  Quelle determinism assures that all control settings are captured at the time that the label is applied to the macro. This further assures that the same search results are returned each time the macro is referenced. Here is an example.

@set %span = 2

in beginning || in_beginning

@set %span = 3

$in_beginning [1] < genesis:1:1

***result:*** none

However, if the user desires the current settings to be used instead, a specialized macro invocation ( #\* ) represents all currently persisted settings; just include it as the last element of the expression (as show below). 

$in_beginning $* < genesis:1:1

***result:*** Gen 1:1 In the beginning, God created ...

Similarly, another specialized invocation ( $0 ) represents default values for all settings. As with all settings, even in the case of these specialized invocations, the last setting per expression wins.

##### Additional explicit macro commands:

Two additional explicit commands exist whereby a macro can be manipulated. We saw above how they can be defined and referenced. There are two additional ways commands that operate on macros: expansion and deletion.  In the last macro definition above where we created  $another-macro, the user could review an expansion by issuing this command:

@review $another-macro

If the user wanted to remove this definition, the @delete action is used.  Here is an example:

@delete $another-macro

If you want the same settings to be persisted to your current session that were in place during macro definition, the @absorb command will persist all settings for the macro into your current session

@absorb $my-favorite-settings-macro 

**NOTES:**

- @absorb also works with command history.
- The two built-in specialized macro invocations ( $\* and $0 ) cannot be deleted or overwritten.

**COMMAND HISTORY** 

*@history* reveals your previous activity.  To show the last ten searches, type:

*@history*

To reveal the last three searches, type:

*@history* 3

To reveal the last twenty searches, type:

*@history* 20 

To reveal activity using date ranges, type any of:

*@history* > 2023/07/04

*@history* < 2023/07/04

*@history* > 2023/07/04 < 2024/07/04 

All ranges are <u>not</u> inclusive. Therefore, commands on July 4th would never be included in the results above. Incidentally, date ranges also work on the @review command.

**INVOKE**

The *invoke* command works for command-history works exactly the same way as it does for macros.  After issuing a *@history* command, the user might receive a response as follows.

*@history*

1>  @set %span = 7

2>  @set %similarity=85

3> eternal power

And the invoke command can re-invoke any command listed.

$3

would be shorthand to re-invoke the search specified as:

eternal power

Again, *invoking* command from your command history is *invoked* just like a macro. Moreover, as with macros, control settings are persisted within your command history to provide invocation determinism. That means that control settings that were in place during the original command are utilized for the invocation.

##### Invoking a command remembers all settings, except when multiple macros are compounded:

Command history captures all settings. We have already discussed macro-determinism. Invoking commands by their review numbers behave exactly like macros. In other words, invoking command history never persists changes into your environment, unless you explicitly request such behavior with the @absorb command.

**RESETTING COMMAND HISTORY**

The @history command can be used to delete <u>all</u> command history.

To clear all command history:

@history -reset

Date [less-than / greater-than] parameters can limit the reset command. Alternatively, Id [less-than / greater-than] parameters can limit the reset command.

### X. Program Help

*@help*

This will provide a help message in a Quelle interpreter.

Or for specific topics:

*@help* find

*@help* set

@help export

etc ...

### XI. Exiting Quelle

Type this to terminate the Quelle interpreter:

*@exit*

### XII. Control Settings & additional related commands

| Verb       | Action Type | Syntax Category | Parameters                         |
| ---------- | :---------: | --------------- | ---------------------------------- |
| **@clear** |  explicit   | CONTROL         | *setting* or ALL                   |
| **@get**   |  explicit   | CONTROL         | **optional:** *setting* or VERSION |

**TABLE 12-1** -- **Listing of additional CONTROL actions**



**Export Format Options:**

| **Markdown**                             | **Text** (UTF8)                       | HTML             | JSON             | YAML           |
| ---------------------------------------- | ------------------------------------- | ---------------- | ---------------- | -------------- |
| *%format = md* <br/>*%format = markdown* | *%format = text*<br/>*%format = utf8* | *%format = html* | *%format = json* | %formal = yaml |

**TABLE 12-2** -- **set** format command can be used to set the default content-formatting for for use with the export verb



| **example**        | **explanation**                                              | shorthand equivalent |
| ------------------ | ------------------------------------------------------------ | -------------------- |
| **@set** %span = 8 | Assign a control setting                                     | @span = 8            |
| **@get** %span     | get a control setting                                        |                      |
| **@clear** %span   | Clear the control setting; restoring the Quelle driver default setting |                      |

**TABLE 12-3** -- **set/clear/get** action operate on configuration settings



**SETTINGS:**

Otherwise, when multiple clauses contain the same setting, only the last setting in the list is preserved.  Example:

%format=md %format=default %format=text

@get format

The final command would return text.  We call this: "last assignment wins". However, there is one caveat to this precedence order: regardless of where in the statement a macro or history invocation is provided within a statement, it never has precedence over a setting that is actually visible within the statement.  Consider this sequence as an example:

%similarity=none || precedence_example

%similarity=85  $precedence_example

@get similarity

The final command would return 85, because it was visible in the compound statement.

AVX-Quelle manifests five control names. Each allows all three actions: ***set***, ***clear***, and ***@get*** verbs. Table 12-4 lists all settings available in AVX-Quelle. AVX-Quelle can support two distinct orthographies [i.e. Contemporary Modern English (avx/modern), and/or Early Modern English (avx/kjv).

| Setting    | Meaning                                                      | Values                                                       | Default Value |
| ---------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------- |
| span       | proximity distance limit                                     | 0 to 999 or verse                                            | 0 [verse]     |
| lexicon    | the lexicon to be used for the searching                     | av/avx/dual (kjv/modern/both)                                | dual (both)   |
| display    | the lexicon to be used for display/rendering                 | av/avx (kjv/modern)                                          | av (kjv)      |
| format     | format of results on output                                  | see Table 12-2                                               | json          |
| similarity | fuzzy phonetics matching threshold is between 1 and 99<br/>0 or *none* means: do not match on phonetics (use text only)<br/>100 or *exact* means that an *exact* phonetics match is expected | 33 to 99 [fuzzy] **or** ...<br>0 **or** *none*<br>100 **or** *exact*<br>Exclamation ( ! ) after the value enables lemma matching (see Section II / item #3) | 0 (none)      |
| VERSION    | Not really a true setting: it works with the @get command to retrieve the revision number of the Quelle grammar supported by AV-Engine. This value is read-only. | 2.0.w.xyz                                                    | n/a           |
| ALL        | Not really a true setting: it works with the @clear command to reset all variables above to their default values. It is only a valid option for the @clear command. | n/a                                                          | n/a           |

**TABLE 12-4** -- **Summary of AVX-Quelle Control Names**

The *@get* command fetches these values. The *@get* command requires a single argument. Examples are below:

*@get* %span

@get %format

**TABLE 12-5** -- **Collective CONTROL operations**

All settings can be cleared using an explicit command:

@clear

**Scope of Settings**

It should be noted that there is a distinction between **@set** and and implicit **assign** syntax. The first form is and explicit command and is persistent (it affects all subsequent statements). Contrariwise, an implicit **assign** affects only the single statement wherewith it is executed. We refer to this as persistence vs assignment.

### XIII. Miscellaneous Information

**QUERYING DRIVER FOR VERSION INFORMATION**

THis command reveals the current Quelle version of the command interpreter:

@get %version

---

In general, AVX-Quelle can be thought of as a stateless server. The only exceptions of its stateless nature are:

1) non-default settings assigned using the **@set** command
2) defined macro labels. 
3) command history

Finally the plus ( + ) delimiter is required between any multi-expression search command. This defines the boundary between the two expressions.  The + delimiter also has it's own distinct settings; any assignments to the left of the + sign are disregarded in subsequent expressions. While results are collated into a single set of search results, each search expression, including its dedicated assignments represents a discrete Quelle search.

---

An object model to manifest the Quelle grammar is depicted below:

![QCommand](./QCommand.png)

### Appendix A. Glossary of Quelle Terminology

**Syntax Categories:** Each syntax category defines rules by which verbs can be expressed in the statement. 

**Actions:** Actions are complete verb-clauses issued in the imperative [you-understood].  Many actions have one or more parameters.  But just like English, a verb phrase can be a single word with no explicit subject and no explicit object.  Consider this English sentence:

Go!

The subject of this sentence is "you understood".  Similarly, all Quelle verbs are issued without an explicit subject. The object of the verb in the one word sentence above is also unstated.  Quelle operates in an analogous manner.  Consider this English sentence:

Go Home!

Like the earlier example, the subject is "you understood".  The object this time is defined, and insists that "you" should go home.  Some verbs always have objects, others sometimes do, and still others never do. Quelle follows this same pattern and some Quelle verbs require direct-objects; and some do not.  In the various tables throughout this document, required and optional parameters are identified, These parameters represent the object of the verb within each respective table.

**Statement**: A statement is composed of one or more segments. When there is more than one segment, each segment contains a search expression. All search results are logically OR’ed together. It is recommended that statements with multiple segments be used sparingly as they represent a complex search pattern. In most situations, a single segment is sufficient to perform very powerful searches. This is because Quelle search expressions already offer powerfull searches with Boolean search logic on all search fragments.

**Expression**: Each segment has zero or one search expressions. A segment without a search expression is typically used to define a partial macro. If you are searching, your segment will contain a search expression. Expressions have fragments, and fragments have features. For an expression to match, all fragments must match (Logical AND). For a fragment to match, any feature must match (Logical OR). AND is represented by &. OR is represented by |.

**Unquoted SEARCH clauses:** an unquoted search clause contains one or more search fragments. If there is more than one fragment in the clause, then each fragment is logically AND’ed together.

**Quoted SEARCH clauses:** a quoted clause contains a single string of terms to search. An explicit match on the string is required. However, an ellipsis ( … ) can be used to indicate that other terms may silently intervene within the quoted string.

- It is called *quoted,* as the entire clause is sandwiched on both sides by double-quotes ( " )
- The absence of double-quotes means that the statement is unquoted

**Booleans and Negations:**

**and:** In Boolean logic, **and** means that all terms must be found. With Quelle, *and* is represented by terms that appear within an unquoted clause. 

**or:** In Boolean logic, **or** means that any term constitutes a match. With Quelle, *or* is represented by the plus (+) between SEARCH expressions. All search results are collated together as a union. 

**not:** In Boolean logic, means that the feature must not be found. With Quelle, *not* is represented by the hyphen ( **-** ) and applies to individual features within a fragment of a search expression. It is best used in conjunction with other features, because any non-match will be included in results. 

hyphen ( **-** ) means that any non-match satisfies the search condition. Used by itself, it would likely return every verse. Therefore, it should be used judiciously.

### Appendix B. Specialized Search tokens in AVX-Quelle

The lexical search domain of AVX-Quelle includes all words in the original KJV text. It can also optionally search using a modernized lexicon of the KJV (e.g. hast and has; this is controllable with the %lexicon setting).  The table below lists additional linguistic extensions available in AVX-Quelle, which happens to be a Level-II Quelle implementation.

| Search Term        | Operator Type                           | Meaning                                                      | Maps To                                                      | Mask   |
| ------------------ | --------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------ |
| un\*               | wildcard (example)                      | starts with: un                                              | all lexicon entries that start with "un"                     | 0x3FFF |
| \*ness             | wildcard (example)                      | ends with: ness                                              | all lexicon entries that end with "ness"                     | 0x3FFF |
| un\*ness           | wildcard (example)                      | starts with: un<br/>ends with: ness                          | all lexicon entries that start with "un", and end with "ness" | 0x3FFF |
| \*profit\*         | wildcard (example)                      | contains: profit                                             | all lexicon entries that contain both "profit"               | 0x3FFF |
| \*pro\*fit\*       | wildcard (example)                      | contains: pro and fit                                        | all lexicon entries that contain both "pro" and "fit" (in any order) | 0x3FFF |
| un\*profit*ness    | wildcard (example)                      | starts with: un<br/>contains: profit<br/>ends with: ness     | all lexicon entries that start with "un", contain "profit", and end with "ness" | 0x3FFF |
| un\*pro\*fit\*ness | wildcard (example)                      | starts with: un<br/>contains: pro and fit<br/>ends with: ness | all lexicon entries that start with "un", contain "pro" and "fit", and end with "ness" | 0x3FFF |
| ~ʃɛpɝd*            | phonetic wildcard (example)             | Tilde marks the wildcard as phonetic (wildcards never perform sounds-alike searching) | All lexical entries that start with the sound ʃɛpɝd (this would include shepherd, shepherds, shepherding...) |        |
| ~ʃɛpɝdz            | sounds-alike search using IPA (example) | Tilde marks the search term as phonetic (and if similarity is set between 33 and 99, search handles approximate matching) | This would match the lexical entry "shepherds" (and possibly similar terms, depending on similarity threshold) |        |
| \\is\\             | lemma                                   | search on all words that share the same lemma as is: be, is, are, art, ... | be\|is\|are\|art\|...                                        | 0x3FFF |
| /noun/             | lexical marker                          | any word where part of speech is a noun                      | POS12::0x010                                                 | 0x0FF0 |
| /n/                | lexical marker                          | synonym for /noun/                                           | POS12::0x010                                                 | 0x0FF0 |
| /verb/             | lexical marker                          | any word where part of speech is a verb                      | POS12::0x100                                                 | 0x0FF0 |
| /v/                | lexical marker                          | synonym for /verb/                                           | POS12::0x100                                                 | 0x0FF0 |
| /pronoun/          | lexical marker                          | any word where part of speech is a pronoun                   | POS12::0x020                                                 | 0x0FF0 |
| /pn/               | lexical marker                          | synonym for /pronoun/                                        | POS12::0x020                                                 | 0x0FF0 |
| /adjective/        | lexical marker                          | any word where part of speech is an adjective                | POS12::0xF00                                                 | 0x0FFF |
| /adj/              | lexical marker                          | synonym for /adjective/                                      | POS12::0xF00                                                 | 0x0FFF |
| /adverb/           | lexical marker                          | any word where part of speech is an adverb                   | POS12::0xA00                                                 | 0x0FFF |
| /adv/              | lexical marker                          | synonym for /adverb/                                         | POS12::0xA00                                                 | 0x0FFF |
| /determiner/       | lexical marker                          | any word where part of speech is a determiner                | POS12::0xD00                                                 | 0x0FF0 |
| /det/              | lexical marker                          | synonym for /determiner/                                     | POS12::0xD00                                                 | 0x0FF0 |
| /preposition/      | lexical marker                          | any word where part of speech is a preposition               | POS12::0x400                                                 | 0x0FF0 |
| /prep/             | lexical marker                          | any word where part of speech is a preposition               | POS12::0x400                                                 | 0x0FF0 |
| /1p/               | lexical marker                          | any word where it is inflected for 1st person (pronouns and verbs) | POS12::0x100                                                 | 0x3000 |
| /2p/               | lexical marker                          | any word where it is inflected for 2nd person (pronouns and verbs) | POS12::0x200                                                 | 0x3000 |
| /3p/               | lexical marker                          | any word where it is inflected for 3rd person (pronouns, verbs, and nouns) | POS12::0x300                                                 | 0x3000 |
| /singular/         | lexical marker                          | any word that is known to be singular (pronouns, verbs, and nouns) | POS12::0x400                                                 | 0xC000 |
| /plural/           | lexical marker                          | any word that is known to be plural (pronouns, verbs, and nouns) | POS12::0x800                                                 | 0xC000 |
| /WH/               | lexical marker                          | any word that is a WH word (e.g., Who, What, When, Where, How) | POS12::0xC00                                                 | 0xC000 |
| /BoB/              | transition marker                       | any word where it is the first word of the book (e.g. first word in Genesis) | TRAN::0xE0                                                   | 0xF0   |
| /BoC/              | transition marker                       | any word where it is the first word of the chapter           | TRAN::0x60                                                   | 0xF0   |
| /BoV/              | transition marker                       | any word where it is the first word of the verse             | TRAN::0x20                                                   | 0xF0   |
| /EoB/              | transition marker                       | any word where it is the last word of the book (e.g. last word in revelation) | TRAN::0xF0                                                   | 0xF0   |
| /EoC/              | transition marker                       | any word where it is the last word of the chapter            | TRAN::0x70                                                   | 0xF0   |
| /EoV/              | transition marker                       | any word where it is the last word of the verse              | TRAN::0x30                                                   | 0xF0   |
| /Hsm/              | segment marker                          | Hard Segment Marker (end) ... one of \. \? \!                | TRAN::0x40                                                   | 0x07   |
| /Csm/              | segment marker                          | Core Segment Marker (end) ... \:                             | TRAN::0x20                                                   | 0x07   |
| /Rsm/              | segment marker                          | Real Segment Marker (end) ... one of \. \? \! \:             | TRAN::0x60                                                   | 0x07   |
| /Ssm/              | segment marker                          | Soft Segment Marker (end) ... one of \, \; \( \) --          | TRAN::0x10                                                   | 0x07   |
| /sm/               | segment marker                          | Any Segment Marker (end)  ... any of the above               | TRAN::!=0x00                                                 | 0x07   |
| /_/                | punctuation                             | any word that is immediately marked for clausal punctuation  | PUNC::!=0x00                                                 | 0xE0   |
| /!/                | punctuation                             | any word that is immediately followed by an exclamation mark | PUNC::0x80                                                   | 0xE0   |
| /?/                | punctuation                             | any word that is immediately followed by a question mark     | PUNC::0xC0                                                   | 0xE0   |
| /./                | punctuation                             | any word that is immediately followed by a period (declarative) | PUNC::0xE0                                                   | 0xE0   |
| /-/                | punctuation                             | any word that is immediately followed by a hyphen/dash       | PUNC::0xA0                                                   | 0xE0   |
| /;/                | punctuation                             | any word that is immediately followed by a semicolon         | PUNC::0x20                                                   | 0xE0   |
| /,/                | punctuation                             | any word that is immediately followed by a comma             | PUNC::0x40                                                   | 0xE0   |
| /:/                | punctuation                             | any word that is immediately followed by a colon (information follows) | PUNC::0x60                                                   | 0xE0   |
| /'/                | punctuation                             | any word that is possessive, marked with an apostrophe       | PUNC::0x10                                                   | 0x10   |
| /)/                | parenthetical text                      | any word that is immediately followed by a close parenthesis | PUNC::0x0C                                                   | 0x0C   |
| /(/                | parenthetical text                      | any word contained within parenthesis                        | PUNC::0x04                                                   | 0x04   |
| /Italics/          | text decoration                         | italicized words marked with this bit in punctuation byte    | PUNC::0x02                                                   | 0x02   |
| /Jesus/            | text decoration                         | words of Jesus marked with this bit in punctuation byte      | PUNC::0x01                                                   | 0x01   |
| /delta/            | lexicon                                 | [archaic] word can be transformed into modern American English |                                                              |        |
| \#FFFF             | PN+POS(12)                              | hexadecimal representation of bits for a PN+POS(12) value.   | See Digital-AV SDK                                           | uint16 |
| \#FFFFFFFF         | POS(32)                                 | hexadecimal representation of bits for a POS(32) value.      | See Digital-AV SDK                                           | uint32 |
| #string            | nupos-string                            | NUPOS string representing part-of-speech. This is the preferred syntax over POS(32), even though they are equivalent. NUPOS part-of-speech values have higher fidelity than the 16-bit PN+POS(12) representations. | See Part-of-Speech-for-Digital-AV.docx                       | uint32 |
| 99999:H            | Strongs Number                          | decimal Strongs number for the Hebrew word in the Old Testament | One of Strongs\[4\]                                          | 0x7FFF |
| 99999:G            | Strongs Number                          | decimal Strongs number for the Greek word in the New Testament | One of Strongs\[4\]                                          | 0x7FFF |

### Appendix C. Object Model to support search tokens in Quelle-AVX

An object model to support specialized Search Tokens for Quelle-AVX is depicted below:

![QFind](./QFind.png)



### Appendix D. Developer Notes

- Quelle expressions have one or more Quelle commands or segments
- Search segments utilize the QFind class
- Search segments have one or more fragments separated by whitespace (all fragments are implicit AND conditions)
- Fragments have one or more MatchAny objects [MatchAny objects are explicit AND conditions, separated by & ]
- MatchAny objects have one or more Features [Features are explicit OR conditions, separated by | ]
- Features can be (negated) using the unary negation operator [ - ]

#### Example

**%span = 15 Lord\* -/v/ & -/adv/ + /v/|/n/&-run**

Two search segments (search segments are encapsulated by the QFind class):

​	search-segment-1: **Lord\* -/v/ & -/adv/**       (this has two fragments)

​		fragment-1 has a single MatchAny object

​			MatchAny object has one [wildcard] feature: **Lord***

​		fragment-2 has two oMatchAny objects (these are AND conditions)

​			MatchAny object-1 has a single negated feature: **-/v/** (NOT a VERB)

​			MatchAny object-2 has a single negated feature: **-/adv/** (NOT an ADVERB)

​	search-segment-2: **/v/|/n/ & -run** has a single fragment

​		The fragment has  two ANDed MatchAny objects

​			MatchAny object-1 has two ORed features: (IS a VERB) OR (IS a NOUN)

​			MatchAny object-2 has a single negated feature: (NOT the word RUN) 

NOTE: the plus sign between the two search segments above implies that results of the segments are ORed together.

​	

