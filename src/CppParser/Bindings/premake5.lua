include ("CSharp")
-- TODO: fix the C# parser bindings on win
if EnabledCLIProjects() and os.ishost("windows") then

include ("CLI")

end