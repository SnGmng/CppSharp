#include "../Tests.h"
#include <string>

class DLL_API Foo
{
public:
    class Vfoo
    {

    };

    // HACK: don't delete this - its external symbols linked by lld crash on Unix
    // https://bugs.llvm.org/show_bug.cgi?id=48325
    Foo();
    ~Foo();
    virtual int vfoo();
    virtual int vbar();

    virtual int append();
    virtual int append(int a);
    int callVirtualWithParameter(int a);
    std::string s;
};

DLL_API int FooCallFoo(Foo* foo);

class DLL_API BaseClassVirtual
{
public:
    BaseClassVirtual();
    BaseClassVirtual(const BaseClassVirtual& other);
    static int virtualCallRetInt(BaseClassVirtual* base);
    virtual int retInt();
    static BaseClassVirtual getBase();
    static BaseClassVirtual* getBasePtr();
    static const char* getTypeName();
};

class DLL_API DerivedClassVirtual : public BaseClassVirtual
{
public:
    // HACK: don't delete this - its external symbols linked by lld crash on Unix
    // https://bugs.llvm.org/show_bug.cgi?id=48325
    DerivedClassVirtual();
    virtual int retInt() override;
};
