# vFlow Project

## Development Container Setup

This project uses a development container configured with VS Code Remote - Containers.

### Pre-Container Setup

Before opening the development container, ensure the entrypoint scripts have execute permissions. You can do this by running the following command:

```sh
chmod +x ./.entrypoint/entrypoint.sh ./.entrypoint/wait-for-it.sh
