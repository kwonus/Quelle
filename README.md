# Quelle
Quelle is a specification. The most generic form of the spec is known as Vanilla-Quelle. Since Quelle could serve as a standard search syntax across various domains, tailoring for specific domains will inevitably produce various dialects. It is recommended that each dialect conform to the Vanilla-Quelle specification. A user-manual-of-sorts is provided here:

[https://github.com/kwonus/Quelle/blob/main/VanillaQuelle.md](https://github.com/kwonus/Quelle/blob/main/Vanilla-Quelle.md)

Initially, the manual served as the most complete specification. It was a specification-by-example (SBE). Now: a [PEG](https://en.wikipedia.org/wiki/Domain-specific_language) grammar exists. This grammar not only provides reference implementation in Rust, but it also provides a formal specification for Vanilla-Quelle:

[https://github.com/kwonus/Quelle/blob/main/rust/src/quelle.pest](https://github.com/kwonus/Quelle/blob/main/rust/src/quelle.pest)

Every attempt is being made to keep the Quelle syntax agnostic about the search domain. Consequently, Quelle syntax has the potential for ubiquity.
<br/></br>

Developer notes, now obsolete, may still be useful and can be found at at:</br>

https://github.com/kwonus/Quelle-Obsolete/blob/master/Quelle-Developer-Notes.md

<br/></br>
# What's Ahead?
- The reference implementation will be made intereactive and report the results of the parse via a console-based/command-line interface
- Tandem projects [search dialects] are expected to become siblings of the reference Rust implementation in this github repo.
