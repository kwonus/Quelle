# Quelle
Quelle is a specification. The most generic form of the spec is known as Vanilla-Quelle. Since Quelle could serve as a standard search syntax across various domains, tailoring for specific domains will inevitably produce various dialects. It is recommended that each dialect conform to the Vanilla-Quelle specification. A user-manual-of-sorts is provided here:

[Quelle/VanillaQuelle.md (github.com/kwonus)](https://github.com/kwonus/Quelle/blob/main/Vanilla-Quelle.md)

Initially, the manual served as the most complete specification. It was a specification-by-example (SBE). Now: a [PEG](https://bford.info/pub/lang/peg.pdf) grammar exists. This grammar not only provides a reference implementation using [Pest](https://pest.rs/) crate in Rust, but it also provides a formal specification:

[Pin-Shot-Blue/src/quelle.pest (github.com/kwonus)](https://github.com/kwonus/Pin-Shot-Blue/blob/main/src/quelle.pest)

Every attempt is being made to keep the Quelle syntax agnostic about the search domain. Consequently, Quelle syntax has the potential for ubiquity.

That said, a dialect of Quelle, optimized for the Digital-AV, can be found here:

[Quelle/Quelle-AVX.md (github.com/kwonus)](https://github.com/kwonus/Quelle/blob/main/Quelle-AVX.md)

[Pin-Shot-Blue/src/avx-quelle.pest (github.com/kwonus)](https://github.com/kwonus/Pin-Shot-Blue/blob/main/src/avx-quelle.pest)





<br/></br>
# What's Available?
- A REST implementation is available in the project named [pinshot-svc](https://github.com/kwonus/pinshot-SVC) in my github repo
- A DLL (library) implementation is available in the project named [pinshot-blue](https://github.com/kwonus/pinshot-blue) in my github repo
