#!/bin/sh

cd $(dirname "$0")
MYDIR=$(pwd)

#export MONO_PREFIX=/opt/mono5-sil
#export MONO_RUNTIME=v4.0.30319
export MONO_DEBUG=explicit-null-checks
export MONO_ENV_OPTIONS="-O=-gshared"
export MONO_PATH=$MYDIR/bin/Debug:/usr/lib/cli/glib-sharp-3.0/:/usr/lib/cli/gtk-sharp-3.0/:/usr/lib/cli/gdk-sharp-3.0/:/usr/lib/cli/atk-sharp-3.0/:/usr/lib/cli/gio-sharp-3.0/:/usr/lib/cli/cairo-sharp-1.10


export LD_PRELOAD=$MYDIR/bin/Debug/Firefox/libgeckofix.so
export LD_LIBRARY_PATH=$MYDIR/bin/Debug/Firefox:$LD_LIBRARY_PATH 

exec /opt/mono5-sil/bin/mono-sgen --debug $MYDIR/bin/Debug/TestGeckofx60.exe
