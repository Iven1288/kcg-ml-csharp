# How to run NanoGpt

## Run NanoGpt on Linux:

1. ensure that you have installed the nvida driver, check with the command "nvidia-smi".
2. install cuda referring to [Nvdia CUDA installation guide](https://developer.nvidia.com/cuda-11-6-0-download-archive?target_os=Linux&target_arch=x86_64&Distribution=Ubuntu&target_version=20.04&target_type=deb_local)
3. install dotnet:

   1) source /etc/os-release
   2) wget https://packages.microsoft.com/config/$ID/$VERSION_ID/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   3) sudo dpkg -i packages-microsoft-prod.deb
   4) rm packages-microsoft-prod.deb
   5) sudo apt update
   6) sudo apt-get install -y dotnet-sdk-7.0
4. prepare dotnet package for building:
   dotnet add package System.CommandLine --version 2.0.0-beta1.21308.1
5. build & run NanoGpt:

   1) replace "TorchSharp-cuda-window" with "TorchSharp-cuda-linux" in kcg-ml-csharp/nano-gpt/nano-gpt.csproj
   2) cd kcg-ml-csharp/cmd  && dotnet build
   3) dotnet run  --running-mode <inference | training> --training-dataset /path/to/txt/data --model-output-directory /path/to/nanogpt/weight/data/dir --device <CPU | CUDA>
