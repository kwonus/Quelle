# Quelle
Quelle is a specification. The most generic form of the spec is known as Vanilla-Quelle. Since Quelle could serve as a standard search syntax across various domains, tailoring for specific domains will inevitably produce various dialects. It is recommended that each dialect conform to the Vanilla-Quelle specification. A user-manual-of-sorts is provided here:

[https://github.com/kwonus/Quelle/blob/main/VanillaQuelle.md](https://github.com/kwonus/Quelle/blob/main/Vanilla-Quelle.md)

Initially, the manual served as the most complete specification. It was a specification-by-example (SBE). Now: a [PEG](https://en.wikipedia.org/wiki/Domain-specific_language) grammar exists. This grammar not only provides a reference implementation in Rust, but it also provides a formal specification for Vanilla-Quelle:

[https://github.com/kwonus/Quelle/blob/main/rust/src/quelle.pest](https://github.com/kwonus/Quelle/blob/main/rust/src/quelle.pest)

Every attempt is being made to keep the Quelle syntax agnostic about the search domain. Consequently, Quelle syntax has the potential for ubiquity.

That said, a dialect of Quelle, optimized for the Digital-AV, can be found here:

[https://github.com/kwonus/Quelle/blob/main/VanillaQuelle.md](https://github.com/kwonus/Quelle/blob/main/Quelle-AVX.md)
[https://github.com/kwonus/Quelle/blob/main/rust/src/quelle-avx.pest](https://github.com/kwonus/Quelle/blob/main/rust/src/quelle-avx.pest)



<br/></br>
# What's Ahead?
- The reference implementation will be made interactive and report the results of the parse via a REST interface
- A Web interface that includes this parsing engine will be incorporated into [Pin-Shot-Blue.io](https://Pin-Shot-Blue.io): Parsing as a Service to be available in 2023.
