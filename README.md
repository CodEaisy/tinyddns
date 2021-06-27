
# TinyDdns

I was tired of managing my ddns setup on Namecheap manually, and all the options I could find aren't easy either, So I built mine.

## How to Use

Only Namecheap is supported for now, and configuration is straight-forward.

```bash
dotnet watch --project src/TinyDdns.csproj -- \
    --DdnsOptions:Provider Namecheap \
    --DdnsOptions:Domains lab.barakimam.me,*.lab.barakimam.me \
    --NamecheapOptions:Domain barakimam.me \
    --NamecheapOptions:Password hello
```
